using LoanApplications.Core.ViewModels;

namespace LoanApplications.Core;

public interface ILoanApplicationService
{
    Task<LoanApplicationResponse> AddLoanApplication(LoanApplicationRequest request,
        CancellationToken cancellationToken);
    Task<LoanApplicationResponse?> GetLoanApplication(Guid id, CancellationToken cancellationToken);
}