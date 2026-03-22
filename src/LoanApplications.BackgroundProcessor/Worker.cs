using LoanApplications.Core;
using LoanApplications.Core.Data;

namespace LoanApplications.BackgroundProcessor;

public class Worker(IServiceProvider serviceProvider, ILogger<Worker> logger) : BackgroundService
{
    private const int DelayInSeconds = 60; // real world make config
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            logger.LogInformation("Checking for new loan applications.");
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = serviceScope.ServiceProvider.GetRequiredService<ILoanEligibilityService>();
                var pendingApplications = await GetPendingLoanApplicationsAsync(stoppingToken, service);
                foreach (var application in pendingApplications)
                {
                    logger.LogInformation("Processing loan application with id: {id}", application.Id);
                    await service.CheckEligibilityAsync(application);
                    logger.LogInformation("Loan application with id: {id} processed.", application.Id);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(DelayInSeconds), stoppingToken);
        }
    }

    private async Task<List<LoanApplication>> GetPendingLoanApplicationsAsync(CancellationToken cancellationToken, ILoanEligibilityService service)
    {
        List<LoanApplication> pendingApplications = [];
        try
        {
            pendingApplications = await service.GetPendingApplicationsAsync(cancellationToken);
            if (pendingApplications.Count == 0)
            {
                logger.LogInformation("No new loan applications found.");
            }
            else
            {
                logger.LogInformation("Found {count} new loan applications.", pendingApplications.Count);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error checking for new loan applications.");
        }
        return pendingApplications;
    }
}