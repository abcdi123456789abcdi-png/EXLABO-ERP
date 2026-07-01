using System;

namespace EXLABO.Core.Entities
{
    public class StockMovement
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime MovementDate { get; set; } = DateTime.Now;
        public string? Reference { get; set; }
        public string? UserName { get; set; }
        public Item? Item { get; set; }
    }
}