using LoanApplications.Core.Data;
using LoanApplications.Core.Enums;

namespace LoanApplications.Core.Mapping;

internal static class DecisionLogEntryMapping
{
    internal static DecisionLogEntry ToDecisionLogEntry(this LoanApplication loanApplication, LoanApplicationRule rule, bool passed,
        DateTime evaluatedAt, string? message)
    {
        return new DecisionLogEntry
        {
            Id = Guid.NewGuid(),
            LoanApplication = loanApplication,
            RuleName = rule,
            Passed = passed,
            EvaluatedAt = evaluatedAt,
            Message = message
        };
    }
}