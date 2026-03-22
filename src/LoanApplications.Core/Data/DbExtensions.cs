using LoanApplications.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace LoanApplications.Core.Data;

public static class DbExtensions
{
    extension(LoanApplicationDbContext context)
    {
        public async Task< List<LoanApplication>> GetPendingApplications(CancellationToken cancellationToken)
        {
            return await context.LoanApplications
                .Where(l => l.Status == LoanStatus.Pending)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HaveAllDecisionsPassed(LoanApplication loanApplication, CancellationToken cancellationToken)
        {
            return await context.DecisionLogEntries
                .AsNoTracking()
                .Where(e => e.LoanApplication == loanApplication)
                .AllAsync(e => e.Passed, cancellationToken: cancellationToken);
        }
    }
}