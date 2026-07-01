using Microsoft.AspNetCore.Mvc;
using EXLABO.Infrastructure.Database;
using EXLABO.Core.Entities;
using EXLABO.Core.DTO;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EXLABO.Web.Controllers
{
    public class IssueController : Controller
    {
        private readonly EXLABOContext db;
        public IssueController(EXLABOContext context) => db = context;

        public IActionResult Create()
        {
            ViewBag.Customers = db.Customers.Where(c => c.IsActive).ToList();
            ViewBag.Items = db.Items.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromBody] IssueDTO model)
        {
            if (model == null || !model.Items.Any()) return BadRequest("No items selected.");

            var issue = new IssueHeader { CustomerID = model.CustomerID, IssueDate = DateTime.Now, UserName = User.Identity?.Name ?? "warehouse" };
            db.IssueHeaders.Add(issue);
            db.SaveChanges();

            var updatedStocks = new System.Collections.Generic.List<object>();

            foreach (var i in model.Items)
            {
                db.IssueDetails.Add(new IssueDetail { IssueID = issue.IssueID, ItemID = i.ItemID, Quantity = i.Quantity });

                var lots = db.Lots.Where(x => x.ItemID == i.ItemID && x.Quantity > 0).OrderBy(x => x.ExpiryDate).ToList();
                int remainingQty = i.Quantity;
                foreach (var lot in lots)
                {
                    if (remainingQty <= 0) break;
                    if (lot.Quantity >= remainingQty) { lot.Quantity -= remainingQty; remainingQty = 0; }
                    else { remainingQty -= lot.Quantity; lot.Quantity = 0; }
                }

                db.StockMovements.Add(new StockMovement { ItemID = i.ItemID, MovementType = "OUT", Quantity = i.Quantity, MovementDate = DateTime.Now });
                int newStock = db.Lots.Where(x => x.ItemID == i.ItemID).Sum(x => (int?)x.Quantity) ?? 0;
                var itemObj = db.Items.Find(i.ItemID);
                updatedStocks.Add(new { itemName = itemObj?.ItemName, newStock = newStock, isLow = newStock <= itemObj.MinimumStock });
                db.AuditLogs.Add(new AuditLog { Username = issue.UserName, Action = $"Issued {i.Quantity} of {itemObj?.ItemName}", ActionDate = DateTime.Now });
            }
            db.SaveChanges();
            return Ok(new { success = true, message = "Issued Successfully!", updatedStocks = updatedStocks });
        }
    }
}