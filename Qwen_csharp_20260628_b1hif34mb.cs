using Microsoft.EntityFrameworkCore;
using EXLABO.Core.Entities;

namespace EXLABO.Infrastructure.Database
{
    public class EXLABOContext : DbContext
    {
        public EXLABOContext(DbContextOptions<EXLABOContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<IssueHeader> IssueHeaders { get; set; }
        public DbSet<IssueDetail> IssueDetails { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<CompanySetting> CompanySettings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Analyzer> Analyzers { get; set; }
        public DbSet<RandoxProgram> RandoxPrograms { get; set; }
        public DbSet<CustomerProgram> CustomerPrograms { get; set; }
        public DbSet<CustomerAnalyzer> CustomerAnalyzers { get; set; }
        public DbSet<AnalyzerItem> AnalyzerItems { get; set; }
        public DbSet<ProgramItem> ProgramItems { get; set; }
        public DbSet<MaterialRequest> MaterialRequests { get; set; }
        public DbSet<MaterialRequestDetail> MaterialRequestDetails { get; set; }
        public DbSet<Stocktake> Stocktakes { get; set; }
        public DbSet<StocktakeDetail> StocktakeDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IssueHeader>().HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerID);
            modelBuilder.Entity<IssueDetail>().HasOne(x => x.Item).WithMany().HasForeignKey(x => x.ItemID);
            modelBuilder.Entity<Lot>().HasOne(x => x.Item).WithMany(x => x.Lots).HasForeignKey(x => x.ItemID);
            modelBuilder.Entity<Analyzer>().HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentID);
            modelBuilder.Entity<Analyzer>().HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyID);
            modelBuilder.Entity<Item>().HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyID);
            modelBuilder.Entity<Item>().HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentID);

            modelBuilder.Entity<AnalyzerItem>().HasKey(ai => new { ai.AnalyzerID, ai.ItemID });
            modelBuilder.Entity<ProgramItem>().HasKey(pi => new { pi.ProgramID, pi.ItemID });
            modelBuilder.Entity<CustomerProgram>().HasKey(cp => new { cp.CustomerID, cp.ProgramID });
            modelBuilder.Entity<CustomerAnalyzer>().HasKey(ca => new { ca.CustomerID, ca.AnalyzerID });

            modelBuilder.Entity<MaterialRequestDetail>().HasOne(d => d.Request).WithMany(r => r.Details).HasForeignKey(d => d.RequestID);
            modelBuilder.Entity<StocktakeDetail>().HasOne(d => d.Stocktake).WithMany(r => r.Details).HasForeignKey(d => d.StocktakeID);

            base.OnModelCreating(modelBuilder);
        }
    }
}