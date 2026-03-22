using LoanApplications.Core.Data;
using LoanApplications.Core.Enums;
using LoanApplications.Core.ViewModels;

namespace LoanApplications.Core.Mapping;

internal static class LoanApplicationMapping
{
    internal static LoanApplication ToLoanApplication(this LoanApplicationRequest request)
    {
        return new LoanApplication
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            MonthlyIncome = request.MonthlyIncome,
            RequestedAmount = request.RequestedAmount,
            TermMonths = request.TermMonths,
            Status = LoanStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    internal static LoanApplicationResponse ToResponse(this LoanApplication entity)
    {
        return new LoanApplicationResponse
        {
            Id = entity.Id,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }
}