using LoanApplications.Core.Data;
using LoanApplications.Core.Enums;
using LoanApplications.Core.Mapping;

namespace LoanApplications.Core;

public class EligibilityRuleEvaluator : IEligibilityRuleEvaluator
{
    public List<DecisionLogEntry> Evaluate(LoanApplication application)
    {
        var now = DateTime.UtcNow;

        return
        [
            EvaluateIncome(application, now),
            EvaluateRequestedAmount(application, now),
            EvaluateTerm(application, now)
        ];
    }

    private static DecisionLogEntry EvaluateIncome(LoanApplication application, DateTime now)
    {
        var passed = application.MonthlyIncome >= 2000;
        var message = passed
            ? null
            : $"Monthly income must be at least £2,000 - Application income is £{application.MonthlyIncome:N0}";
        return application.ToDecisionLogEntry(LoanApplicationRule.Income, passed, now, message);
    }

    private static DecisionLogEntry EvaluateRequestedAmount(LoanApplication application, DateTime now)
    {
        var maxAmount = application.MonthlyIncome * 4;
        var passed = application.RequestedAmount <= maxAmount;
        var message = passed
            ? null
            : $"Requested amount must be no more than £{maxAmount:N0} (monthly income £{application.MonthlyIncome:N0} x 4) - Application requested £{application.RequestedAmount:N0}";
        return application.ToDecisionLogEntry(LoanApplicationRule.RequestedAmount, passed, now, message);
    }

    private static DecisionLogEntry EvaluateTerm(LoanApplication application, DateTime now)
    {
        var passed = application.TermMonths is >= 12 and <= 60;
        var message = passed
            ? null
            : $"Term must be between 12 and 60 months - Application term is {application.TermMonths} months";
        return application.ToDecisionLogEntry(LoanApplicationRule.Term, passed, now, message);
    }
}
