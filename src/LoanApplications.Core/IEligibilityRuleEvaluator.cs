using LoanApplications.Core.Data;

namespace LoanApplications.Core;

public interface IEligibilityRuleEvaluator
{
    List<DecisionLogEntry> Evaluate(LoanApplication application);
}
