using FluentValidation;
using LoanApplications.Core.Data;
using LoanApplications.Core.Validators;
using LoanApplications.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanApplications.Core;

public static class ConfigurationExtensions
{
    public static void AddLoanApplications(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<LoanApplicationDbContext>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection") ?? "Data Source=loanapplications.db"));
        services.AddSingleton<IValidator<LoanApplicationRequest>, LoanApplicationRequestValidator>();
        services.AddTransient<ILoanApplicationService, LoanApplicationService>();
        services.AddTransient<ILoanEligibilityService, LoanEligibilityService>();
    }
}