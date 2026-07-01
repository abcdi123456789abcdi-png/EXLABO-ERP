namespace EXLABO.Core.Entities
{
    public class ProgramItem
    {
        public int ProgramID { get; set; }
        public int ItemID { get; set; }
        public int DefaultQuantity { get; set; } = 1;
        public RandoxProgram? Program { get; set; }
        public Item? Item { get; set; }
    }
}