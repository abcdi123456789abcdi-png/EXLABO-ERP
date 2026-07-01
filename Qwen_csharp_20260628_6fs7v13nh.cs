using System.Collections.Generic;

namespace EXLABO.Core.Entities
{
    public class RandoxProgram
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string ProgramCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<CustomerProgram>? CustomerPrograms { get; set; }
        public ICollection<ProgramItem>? ProgramItems { get; set; }
    }
}