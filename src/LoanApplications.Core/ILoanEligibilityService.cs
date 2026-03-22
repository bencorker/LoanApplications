using LoanApplications.Core.Data;
using LoanApplications.Core.ViewModels;

namespace LoanApplications.Core;

public interface ILoanEligibilityService
{
    Task CheckEligibilityAsync(LoanApplication loanApplication);
    Task<List<LoanApplication>> GetPendingApplicationsAsync(CancellationToken cancellationToken);
}