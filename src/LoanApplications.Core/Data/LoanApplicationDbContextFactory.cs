using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LoanApplications.Core.Data;

public class LoanApplicationDbContextFactory : IDesignTimeDbContextFactory<LoanApplicationDbContext>
{
    public LoanApplicationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<LoanApplicationDbContext>()
            .UseSqlite("Data Source=loanapplications.db")
            .Options;

        return new LoanApplicationDbContext(options);
    }
}
