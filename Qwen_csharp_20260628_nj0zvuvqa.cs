using System;

namespace EXLABO.Core.Entities
{
    public class AuditLog
    {
        public int ID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public string? Details { get; set; }
    }
}