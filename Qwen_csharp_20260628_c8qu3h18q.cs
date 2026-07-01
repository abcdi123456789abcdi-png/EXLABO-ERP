using System;
using System.Collections.Generic;

namespace EXLABO.Core.Entities
{
    public class Stocktake
    {
        public int StocktakeID { get; set; }
        public DateTime StocktakeDate { get; set; } = DateTime.Now;
        public string ConductedBy { get; set; } = string.Empty;
        public string Status { get; set; } = "Draft";
        public string? Notes { get; set; }
        public List<StocktakeDetail> Details { get; set; } = new List<StocktakeDetail>();
    }

    public class StocktakeDetail
    {
        public int ID { get; set; }
        public int StocktakeID { get; set; }
        public int ItemID { get; set; }
        public int SystemQuantity { get; set; }
        public int PhysicalQuantity { get; set; }
        public Item? Item { get; set; }
        public Stocktake? Stocktake { get; set; }
    }
}