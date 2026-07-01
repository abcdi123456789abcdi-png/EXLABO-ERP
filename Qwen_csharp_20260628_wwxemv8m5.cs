namespace EXLABO.Core.Entities
{
    public class AnalyzerItem
    {
        public int AnalyzerID { get; set; }
        public int ItemID { get; set; }
        public int DefaultQuantity { get; set; } = 1;
        public Analyzer? Analyzer { get; set; }
        public Item? Item { get; set; }
    }
}