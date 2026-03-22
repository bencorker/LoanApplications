using System.ComponentModel;

namespace LoanApplications.Core.ViewModels;

public record LoanApplicationRequest
{
    [Description("Name of the applicant")]
    public required string Name { get; set; }
    [Description("Email of the applicant")]
    public required string Email { get; set; }
    [Description("Monthly income of the applicant")]
    public required int MonthlyIncome { get; set; }
    [Description("Requested amount of the loan")]
    public required int RequestedAmount { get; set; }
    [Description("Term of the loan in months")]
    public required int TermMonths { get; set; }
}