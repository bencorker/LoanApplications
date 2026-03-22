using LoanApplications.Core.ViewModels;

namespace LoanApplications.Core;

public class LoanApplicationService : ILoanApplicationService
{
    public async Task<LoanApplicationResponse> AddLoanApplication(LoanApplicationRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<LoanApplicationResponse> GetLoanApplication(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}