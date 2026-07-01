using Microsoft.AspNetCore.Mvc;
using EXLABO.Core.Services;
using EXLABO.Infrastructure.Database;
using EXLABO.Core.Entities;
using EXLABO.Core.DTO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EXLABO.Web.Controllers
{
    public class SmartOrderController : Controller
    {
        private readonly AIVisionService _aiService;
        private readonly EXLABOContext _db;
        private readonly IConfiguration _config;

        public SmartOrderController(AIVisionService aiService, EXLABOContext db, IConfiguration config)
        {
            _aiService = aiService; _db = db; _config = config;
        }

        public IActionResult Create()
        {
            ViewBag.Customers = _db.Customers.Where(c => c.IsActive).ToList();
            ViewBag.CurrentUserEmail = "admin@exlabo.com";
            ViewBag.WarehouseEmail = _config["EmailSettings:WarehouseEmail"] ?? "warehouse@exlabo.com";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile, int customerId)
        {
            if (imageFile == null || imageFile.Length == 0) return Json(new { success = false, error = "No image" });
            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            byte[] bytes = ms.ToArray();
            string format = imageFile.ContentType.Split('/').Last();

            try
            {
                var extracted = await _aiService.AnalyzeOrderImageAsync(bytes, format);
                var matched = new System.Collections.Generic.List<MatchedItem>();
                var synonyms = new System.Collections.Generic.Dictionary<string, string> { { "dill", "diluent" }, { "lys", "lyse" }, { "ptt", "ptt reagent" } };

                foreach (var item in extracted)
                {
                    var searchName = item.MaterialName.ToLower();
                    if (synonyms.ContainsKey(searchName)) searchName = synonyms[searchName];
                    var dbItem = _db.Items.FirstOrDefault(i => i.ItemName.ToLower().Contains(searchName) || i.ItemCode.ToLower().Contains(searchName));
                    matched.Add(new MatchedItem { ItemID = dbItem?.ItemID, ItemCode = dbItem?.ItemCode ?? "NOT_FOUND", ItemName = dbItem?.ItemName ?? item.MaterialName, RequestedQuantity = item.Quantity, IsMatched = dbItem != null });
                }
                return Json(new { success = true, items = matched });
            }
            catch (System.Exception ex) { return Json(new { success = false, error = ex.Message }); }
        }

        [HttpPost]
        public IActionResult ReviewOrder(OrderReviewModel model)
        {
            var dbItems = _db.Items.Where(i => model.Items.Select(x => x.ItemID).Contains(i.ItemID)).ToList();
            var summary = new OrderSummary
            {
                CustomerName = _db.Customers.Find(model.CustomerId)?.CustomerName ?? "",
                SenderEmail = model.SenderEmail,
                ReceiverEmail = model.ReceiverEmail,
                Items = model.Items.Select(i => { var db = dbItems.FirstOrDefault(x => x.ItemID == i.ItemID); return new OrderItemSummary { ItemCode = db?.ItemCode ?? "N/A", ItemName = db?.ItemName ?? i.ItemName, Quantity = i.Quantity }; }).ToList()
            };
            return Json(new { success = true, summary = summary });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAndSend(OrderReviewModel model)
        {
            try
            {
                var order = new MaterialRequest { RequestDate = DateTime.Now, RequestedBy = model.SenderEmail, WarehouseEmail = model.ReceiverEmail, SenderEmail = model.SenderEmail, Status = "Confirmed", IsEmailSent = true, CustomerID = model.CustomerId > 0 ? model.CustomerId : null };
                foreach (var item in model.Items) order.Details.Add(new MaterialRequestDetail { ItemID = item.ItemID, RequestedQuantity = item.Quantity });
                _db.MaterialRequests.Add(order);
                await _db.SaveChangesAsync();
                await SendOrderEmailAsync(order);
                return Json(new { success = true, message = "Order sent!" });
            }
            catch (System.Exception ex) { return Json(new { success = false, error = ex.Message }); }
        }

        private async Task SendOrderEmailAsync(MaterialRequest order)
        {
            var smtpHost = _config["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"] ?? "587");
            var smtpUser = _config["EmailSettings:SmtpUser"];
            var smtpPass = _config["EmailSettings:SmtpPass"];
            using var client = new SmtpClient(smtpHost, smtpPort) { Credentials = new NetworkCredential(smtpUser, smtpPass), EnableSsl = true };
            var mail = new MailMessage { From = new MailAddress(smtpUser, "EXLABO ERP"), Subject = $"Order #{order.RequestID}", Body = $"<h2>Order #{order.RequestID}</h2><p>From: {order.RequestedBy}</p><p>To: {order.WarehouseEmail}</p>", IsBodyHtml = true };
            mail.To.Add(order.WarehouseEmail);
            await client.SendMailAsync(mail);
        }
    }
}