using Microsoft.EntityFrameworkCore;

namespace LoanApplications.Core.Data;

public class LoanApplicationDbContext(DbContextOptions<LoanApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();
    public DbSet<DecisionLogEntry> DecisionLogEntries => Set<DecisionLogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoanApplication>()
            .Property(e => e.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<DecisionLogEntry>()
            .Property(e => e.RuleName)
            .HasConversion<string>();
    }
}
