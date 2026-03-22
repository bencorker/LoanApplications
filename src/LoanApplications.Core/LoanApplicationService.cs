using FluentValidation;
using LoanApplications.Core.Data;
using LoanApplications.Core.Mapping;
using LoanApplications.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LoanApplications.Core;

public class LoanApplicationService(
    LoanApplicationDbContext db,
    IValidator<LoanApplicationRequest> validator) : ILoanApplicationService
{
    public async Task<LoanApplicationResponse> AddLoanApplication(LoanApplicationRequest request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        var entity = request.ToLoanApplication();
        db.LoanApplications.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
        return entity.ToResponse();
    }

    public async Task<LoanApplicationResponse?> GetLoanApplication(Guid id, CancellationToken cancellationToken)
    {
        var entity = await db.LoanApplications.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null) return null;
        return entity.ToResponse();
    }
}
