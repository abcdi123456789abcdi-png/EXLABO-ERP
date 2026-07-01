namespace EXLABO.Core.Entities
{
    public class CustomerProgram
    {
        public int CustomerID { get; set; }
        public int ProgramID { get; set; }
        public Customer? Customer { get; set; }
        public RandoxProgram? Program { get; set; }
    }
}