using LoanApplications.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanApplications.Core.Data;

public static class DbExtensions
{
    public static async Task< List<LoanApplication>> GetPendingApplications(this LoanApplicationDbContext context, CancellationToken cancellationToken)
    {
        return await context.LoanApplications
            .Where(l => l.Status == LoanStatus.Pending)
            .ToListAsync(cancellationToken);
    }
}