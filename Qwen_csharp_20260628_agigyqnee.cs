namespace EXLABO.Core.Entities
{
    public class CustomerAnalyzer
    {
        public int CustomerID { get; set; }
        public int AnalyzerID { get; set; }
        public Customer? Customer { get; set; }
        public Analyzer? Analyzer { get; set; }
    }
}