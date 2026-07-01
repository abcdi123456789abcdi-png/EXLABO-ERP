using System;
using System.Collections.Generic;

namespace EXLABO.Core.Entities
{
    public class IssueHeader
    {
        public int IssueID { get; set; }
        public int CustomerID { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public string UserName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public List<IssueDetail> Details { get; set; } = new List<IssueDetail>();
        public Customer? Customer { get; set; }
    }

    public class IssueDetail
    {
        public int ID { get; set; }
        public int IssueID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public string? LotNumber { get; set; }
        public Item? Item { get; set; }
        public IssueHeader? IssueHeader { get; set; }
    }
}