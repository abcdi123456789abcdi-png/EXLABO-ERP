using Microsoft.AspNetCore.Mvc;
using EXLABO.Infrastructure.Database;
using EXLABO.Core.Entities;
using System.Linq;

namespace EXLABO.Web.Controllers
{
    public class CustomerManagementController : Controller
    {
        private readonly EXLABOContext db;
        public CustomerManagementController(EXLABOContext context) => db = context;

        public IActionResult Index() => View(db.Customers.Where(c => c.IsActive).OrderBy(c => c.CustomerName).ToList());

        [HttpGet]
        public JsonResult SearchCustomers(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Json(new System.Collections.Generic.List<object>());
            term = term.ToLower().Trim();
            var results = db.Customers.Where(c => c.IsActive && (c.CustomerName.ToLower().Contains(term) || c.CustomerNameAr.ToLower().Contains(term) || c.Aliases.ToLower().Contains(term))).Select(c => new { id = c.CustomerID, text = $"{c.CustomerName} {(string.IsNullOrEmpty(c.CustomerNameAr) ? "" : "- " + c.CustomerNameAr)}" }).Take(10).ToList();
            return Json(results);
        }

        [HttpPost]
        public IActionResult AddOrUpdate(Customer model)
        {
            if (model.CustomerID > 0) db.Customers.Update(model); else db.Customers.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var c = db.Customers.Find(id);
            if (c != null) { c.IsActive = false; db.SaveChanges(); }
            return RedirectToAction("Index");
        }
    }
}