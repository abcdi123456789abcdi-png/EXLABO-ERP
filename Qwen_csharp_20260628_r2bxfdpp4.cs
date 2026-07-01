namespace EXLABO.Core.Entities
{
    public class Analyzer
    {
        public int AnalyzerID { get; set; }
        public string AnalyzerName { get; set; } = string.Empty;
        public int DepartmentID { get; set; }
        public int CompanyID { get; set; }
        public Department? Department { get; set; }
        public Company? Company { get; set; }
    }
}