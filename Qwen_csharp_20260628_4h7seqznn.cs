using System.Collections.Generic;

namespace EXLABO.Core.Entities
{
    public class Item
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public int MinimumStock { get; set; } = 50;
        public string Barcode { get; set; } = string.Empty;
        public string QRCode { get; set; } = string.Empty;
        public int CompanyID { get; set; }
        public int DepartmentID { get; set; }
        public string ItemType { get; set; } = string.Empty;
        
        public Company? Company { get; set; }
        public Department? Department { get; set; }
        public ICollection<AnalyzerItem>? AnalyzerItems { get; set; }
        public ICollection<Lot>? Lots { get; set; }
    }
}