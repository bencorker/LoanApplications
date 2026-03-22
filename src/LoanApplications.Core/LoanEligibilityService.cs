using LoanApplications.Core.Data;
using LoanApplications.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LoanApplications.Core;

public class LoanEligibilityService(
    LoanApplicationDbContext db,
    IEligibilityRuleEvaluator ruleEvaluator,
    ILogger<LoanEligibilityService> logger) : ILoanEligibilityService
{
    public async Task CheckEligibilityAsync(LoanApplication loanApplication, CancellationToken cancellationToken)
    {
        loanApplication.DecisionLogEntries ??= [];
        var results = ruleEvaluator.Evaluate(loanApplication);

        foreach (var result in results)
        {
            var existing = loanApplication.DecisionLogEntries.FirstOrDefault(e => e.RuleName == result.RuleName);

            if (existing is not null)
            {
                logger.LogDebug("Rule {Rule} already evaluated for application {Id}, skipping", result.RuleName, loanApplication.Id);
                continue;
            }
            loanApplication.DecisionLogEntries.Add(result);
        }

        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving decision log entries for loan application {Id}", loanApplication.Id);
            return;
        }

        var allPassed = await db.HaveAllDecisionsPassed(loanApplication, CancellationToken.None);

        loanApplication.Status = allPassed ? LoanStatus.Approved : LoanStatus.Rejected;
        loanApplication.ReviewedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Loan application {Id} has been {Status}", loanApplication.Id, loanApplication.Status);
    }

    public async Task<List<LoanApplication>> GetPendingApplicationsAsync(CancellationToken cancellationToken)
    {
        return await db.GetPendingApplications(cancellationToken);
    }
}
