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
        var existingRules = await db.DecisionLogEntries
            .Where(e => e.LoanApplicationId == loanApplication.Id)
            .Select(e => e.RuleName)
            .ToListAsync(cancellationToken);

        var results = ruleEvaluator.Evaluate(loanApplication);

        foreach (var result in results)
        {
            if (existingRules.Contains(result.RuleName))
            {
                logger.LogInformation("Rule {Rule} already evaluated for application {Id}, skipping", result.RuleName, loanApplication.Id);
                continue;
            }
            logger.LogInformation("Evaluated rule {Rule} for application {Id} - Passed {passed}", result.RuleName, loanApplication.Id, result.Passed);
            
            logger.LogInformation("Saving decision log entry for loan application {Id}", loanApplication.Id);
            db.DecisionLogEntries.Add(result);
            logger.LogInformation("Decision log entry saved for loan application {Id}", loanApplication.Id);
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
        try
        {
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Loan application {Id} has been {Status}", loanApplication.Id, loanApplication.Status);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating loan application status for {Id}", loanApplication.Id);
        }
        
    }

    public async Task<List<LoanApplication>> GetPendingApplicationsAsync(CancellationToken cancellationToken)
    {
        return await db.GetPendingApplications(cancellationToken);
    }
}
