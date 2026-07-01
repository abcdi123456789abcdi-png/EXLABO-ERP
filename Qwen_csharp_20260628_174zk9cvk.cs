using System.Collections.Generic;

namespace EXLABO.Core.Entities
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerNameAr { get; set; } = string.Empty;
        public string Aliases { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        public ICollection<CustomerProgram>? CustomerPrograms { get; set; }
        public ICollection<CustomerAnalyzer>? CustomerAnalyzers { get; set; }
    }
}