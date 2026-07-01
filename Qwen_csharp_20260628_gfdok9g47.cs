using Microsoft.AspNetCore.Mvc;
using EXLABO.Infrastructure.Database;
using EXLABO.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EXLABO.Web.Controllers
{
    public class StocktakeController : Controller
    {
        private readonly EXLABOContext db;
        public StocktakeController(EXLABOContext context) => db = context;

        public IActionResult Create()
        {
            ViewBag.Items = db.Items.Select(i => new { i.ItemID, i.ItemCode, i.ItemName, SystemQty = db.Lots.Where(l => l.ItemID == i.ItemID).Sum(l => (int?)l.Quantity) ?? 0 }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveStocktake(System.Collections.Generic.List<StocktakeDetail> details, string notes)
        {
            var stocktake = new Stocktake { StocktakeDate = System.DateTime.Now, ConductedBy = "Admin", Status = "Completed", Notes = notes };
            foreach (var d in details)
            {
                var sysQty = db.Lots.Where(l => l.ItemID == d.ItemID).Sum(l => (int?)l.Quantity) ?? 0;
                stocktake.Details.Add(new StocktakeDetail { ItemID = d.ItemID, SystemQuantity = sysQty, PhysicalQuantity = d.PhysicalQuantity });
                int variance = d.PhysicalQuantity - sysQty;
                if (variance != 0)
                {
                    var lot = db.Lots.Where(l => l.ItemID == d.ItemID).OrderBy(l => l.ExpiryDate).FirstOrDefault();
                    if (lot != null) { lot.Quantity = d.PhysicalQuantity; if (lot.Quantity < 0) lot.Quantity = 0; }
                    db.StockMovements.Add(new StockMovement { ItemID = d.ItemID, MovementType = "ADJUST", Quantity = variance, MovementDate = System.DateTime.Now, Reference = $"Stocktake #{stocktake.StocktakeID}" });
                }
            }
            db.Stocktakes.Add(stocktake);
            await db.SaveChangesAsync();
            TempData["Success"] = "Stocktake completed!";
            return RedirectToAction("Create");
        }
    }
}