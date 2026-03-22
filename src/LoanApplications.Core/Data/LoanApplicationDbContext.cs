using Microsoft.EntityFrameworkCore;

namespace LoanApplications.Core.Data;

public class LoanApplicationDbContext(DbContextOptions<LoanApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();
}
