using Microsoft.AspNetCore.Mvc;
using EXLABO.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace EXLABO.Web.Controllers
{
    public class ExportController : Controller
    {
        private readonly EXLABOContext db;
        public ExportController(EXLABOContext context) { db = context; Settings.License = LicenseKind.Community; }

        public IActionResult MonthlyReport(int? month = null, int? year = null)
        {
            int m = month ?? System.DateTime.Now.Month;
            int y = year ?? System.DateTime.Now.Year;
            var stock = db.Items.Select(i => new { i.ItemName, Stock = db.Lots.Where(l => l.ItemID == i.ItemID).Sum(l => (int?)l.Quantity) ?? 0 }).ToList();
            var customers = db.IssueHeaders.Where(i => i.IssueDate.Month == m && i.IssueDate.Year == y).Select(i => new { CustomerName = i.Customer.CustomerName, Total = i.Details.Sum(d => d.Quantity) }).OrderByDescending(c => c.Total).ToList();

            var pdf = Document.Create(container => {
                container.Page(page => {
                    page.Size(PageSizes.A4); page.Margin(40);
                    page.Header().Text($"EXLABO Monthly Report ({m}/{y})").FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                    page.Content().PaddingVertical(20).Column(col => {
                        col.Item().Text("Current Stock").FontSize(16).Bold();
                        col.Item().PaddingTop(5).Table(table => {
                            table.ColumnsDefinition(c => { c.RelativeColumn(2); c.RelativeColumn(); });
                            table.Header().Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Item").Bold();
                            table.Header().Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Qty").Bold();
                            foreach (var s in stock) { table.Cell().Padding(5).Text(s.ItemName); table.Cell().Padding(5).Text(s.Stock.ToString()); }
                        });
                        col.Item().PaddingTop(20).Text("Customer Activity").FontSize(16).Bold();
                        col.Item().PaddingTop(5).Table(table => {
                            table.ColumnsDefinition(c => { c.RelativeColumn(2); c.RelativeColumn(); });
                            table.Header().Cell().Background(Colors.Blue.Lighten3).Padding(5).Text("Customer").Bold();
                            table.Header().Cell().Background(Colors.Blue.Lighten3).Padding(5).Text("Total").Bold();
                            foreach (var c in customers) { table.Cell().Padding(5).Text(c.CustomerName); table.Cell().Padding(5).Text(c.Total.ToString()); }
                        });
                    });
                });
            }).GeneratePdf();
            return File(pdf, "application/pdf", $"Report_{m}_{y}.pdf");
        }
    }
}