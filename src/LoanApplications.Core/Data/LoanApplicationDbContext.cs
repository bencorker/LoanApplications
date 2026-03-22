using Microsoft.EntityFrameworkCore;

namespace LoanApplications.Core.Data;

public class LoanApplicationDbContext(DbContextOptions<LoanApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoanApplication>()
            .Property(e => e.Status)
            .HasConversion<string>();
    }
}
