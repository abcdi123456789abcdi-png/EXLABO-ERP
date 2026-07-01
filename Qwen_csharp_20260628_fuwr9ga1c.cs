using System;

namespace EXLABO.Core.Entities
{
    public class Lot
    {
        public int LotID { get; set; }
        public int ItemID { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public string? StorageCondition { get; set; }
        public Item? Item { get; set; }
    }
}