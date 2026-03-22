using FluentValidation;
using LoanApplications.Core.Data;
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
        var entity = new LoanApplication
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            MonthlyIncome = request.MonthlyIncome,
            RequestedAmount = request.RequestedAmount,
            TermMonths = request.TermMonths,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        db.LoanApplications.Add(entity);
        await db.SaveChangesAsync(cancellationToken);

        return new LoanApplicationResponse
        {
            Id = entity.Id,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }

    public async Task<LoanApplicationResponse?> GetLoanApplication(Guid id, CancellationToken cancellationToken)
    {
        var entity = await db.LoanApplications.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null) return null;

        return new LoanApplicationResponse
        {
            Id = entity.Id,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }
}
