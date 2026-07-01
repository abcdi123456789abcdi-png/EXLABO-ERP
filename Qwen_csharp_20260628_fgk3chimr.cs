namespace EXLABO.Core.Entities
{
    public class Branch
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? ManagerName { get; set; }
    }
}