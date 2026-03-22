using LoanApplications.Core.Data;

namespace LoanApplications.Core;

public interface ILoanEligibilityService
{
    Task CheckEligibilityAsync(LoanApplication loanApplication, CancellationToken cancellationToken);
    Task<List<LoanApplication>> GetPendingApplicationsAsync(CancellationToken cancellationToken);
}