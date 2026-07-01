using Microsoft.AspNetCore.Mvc;
using EXLABO.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EXLABO.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly EXLABOContext db;
        public DashboardController(EXLABOContext context) => db = context;

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCustomers = await db.Customers.CountAsync(c => c.IsActive);
            ViewBag.TotalItems = await db.Items.CountAsync();
            ViewBag.TotalStock = await db.Lots.SumAsync(l => (int?)l.Quantity) ?? 0;

            var today = DateTime.Now;
            var lowStockItems = await db.Items
                .Where(i => (db.Lots.Where(l => l.ItemID == i.ItemID).Sum(l => (int?)l.Quantity) ?? 0) <= i.MinimumStock)
                .Select(i => new { i.ItemID, i.ItemCode, i.ItemName, CurrentStock = db.Lots.Where(l => l.ItemID == i.ItemID).Sum(l => (int?)l.Quantity) ?? 0, i.MinimumStock })
                .ToListAsync();
            ViewBag.LowStockCount = lowStockItems.Count;
            ViewBag.LowStockItems = lowStockItems;

            var expiredLots = await db.Lots.Where(l => l.ExpiryDate < today).Include(l => l.Item).ToListAsync();
            ViewBag.ExpiredCount = expiredLots.Count;

            var expiringSoon = await db.Lots.Where(l => l.ExpiryDate >= today && l.ExpiryDate <= today.AddDays(90)).Include(l => l.Item).ToListAsync();
            ViewBag.ExpiringSoonCount = expiringSoon.Count;
            ViewBag.ExpiringSoonItems = expiringSoon.Select(l => new { l.Item?.ItemName, l.LotNumber, l.ExpiryDate, DaysLeft = (l.ExpiryDate - today).Days }).OrderBy(x => x.DaysLeft).Take(5).ToList();

            var todayStart = today.Date;
            ViewBag.TodayOrders = await db.IssueHeaders.Where(i => i.IssueDate >= todayStart).CountAsync();
            ViewBag.TodaySmartOrders = await db.MaterialRequests.Where(m => m.RequestDate >= todayStart).CountAsync();
            ViewBag.TotalTodayOrders = ViewBag.TodayOrders + ViewBag.TodaySmartOrders;

            var allLots = await db.Lots.ToListAsync();
            ViewBag.AvailableStock = allLots.Where(l => l.Quantity > 0 && l.ExpiryDate > today.AddDays(90)).Sum(l => l.Quantity);
            ViewBag.LowStockQty = allLots.Where(l => l.Quantity > 0 && l.Quantity <= 50 && l.ExpiryDate > today.AddDays(90)).Sum(l => l.Quantity);
            ViewBag.ExpiringStock = allLots.Where(l => l.Quantity > 0 && l.ExpiryDate <= today.AddDays(90) && l.ExpiryDate >= today).Sum(l => l.Quantity);
            ViewBag.ExpiredStock = allLots.Where(l => l.ExpiryDate < today).Sum(l => l.Quantity);

            ViewBag.RecentOrders = await db.IssueHeaders.Include(i => i.Customer).Include(i => i.Details).OrderByDescending(i => i.IssueDate).Take(5).Select(i => new { i.IssueID, CustomerName = i.Customer != null ? i.Customer.CustomerName : "Unknown", i.IssueDate, TotalItems = i.Details.Sum(d => d.Quantity) }).ToListAsync();
            ViewBag.RecentReceipts = await db.MaterialRequests.OrderByDescending(m => m.RequestDate).Take(5).Select(m => new { m.RequestID, m.RequestedBy, m.RequestDate, m.Status, TotalItems = m.Details.Sum(d => d.RequestedQuantity) }).ToListAsync();

            return View();
        }
    }
}