using System;
using System.Collections.Generic;

namespace EXLABO.Core.Entities
{
    public class MaterialRequest
    {
        public int RequestID { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string RequestedBy { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string WarehouseEmail { get; set; } = "warehouse@exlabo.com";
        public string SenderEmail { get; set; } = string.Empty;
        public bool IsEmailSent { get; set; } = false;
        public int? CustomerID { get; set; }
        public List<MaterialRequestDetail> Details { get; set; } = new List<MaterialRequestDetail>();
        public Customer? Customer { get; set; }
    }

    public class MaterialRequestDetail
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public int ItemID { get; set; }
        public int RequestedQuantity { get; set; }
        public int CurrentStock { get; set; }
        public Item? Item { get; set; }
        public MaterialRequest? Request { get; set; }
    }
}