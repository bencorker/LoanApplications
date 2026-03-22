using FluentValidation;
using LoanApplications.Core.ViewModels;

namespace LoanApplications.Core.Validators;

public class LoanApplicationRequestValidator : AbstractValidator<LoanApplicationRequest>
{
    public LoanApplicationRequestValidator()
    {
        RuleFor(x => x.MonthlyIncome).GreaterThan(0);
        RuleFor(x => x.RequestedAmount).GreaterThan(0);
        RuleFor(x => x.TermMonths).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Email).EmailAddress();
    }
}