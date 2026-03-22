using LoanApplications.Core.Data;
using Microsoft.Extensions.Logging;

namespace LoanApplications.Core;

public class LoanEligibilityService(LoanApplicationDbContext db, ILogger<LoanEligibilityService> logger) : ILoanEligibilityService
{
    public async Task CheckEligibilityAsync(LoanApplication loanApplication)
    {
        
    }

    public async Task<List<LoanApplication>> GetPendingApplicationsAsync(CancellationToken cancellationToken)
    {
        return await db.GetPendingApplications(cancellationToken);
    }
}