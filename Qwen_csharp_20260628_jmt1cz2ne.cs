using System.Collections.Generic;

namespace EXLABO.Core.DTO
{
    public class IssueDTO
    {
        public int CustomerID { get; set; }
        public List<IssueItemDTO> Items { get; set; } = new List<IssueItemDTO>();
    }

    public class IssueItemDTO
    {
        public int ItemID { get; set; }
        public int Quantity { get; set; }
    }

    public class ExtractedItem
    {
        public string MaterialName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class MatchedItem
    {
        public int? ItemID { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int RequestedQuantity { get; set; }
        public bool IsMatched { get; set; }
    }

    public class OrderReviewModel
    {
        public int CustomerId { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }

    public class OrderItemDTO
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class OrderSummary
    {
        public string CustomerName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public List<OrderItemSummary> Items { get; set; } = new List<OrderItemSummary>();
    }

    public class OrderItemSummary
    {
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}