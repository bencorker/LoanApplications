using System.ComponentModel.DataAnnotations;
using LoanApplications.Core.Enums;

namespace LoanApplications.Core.Data;

public class DecisionLogEntry
{
    public LoanApplication? LoanApplication { get; set; }
    public Guid Id { get; set; }
    public LoanApplicationRule RuleName { get; set; }
    public bool Passed { get; set; }
    [MaxLength(255)]
    public string? Message { get; set; }
    public DateTime EvaluatedAt { get; set; }
}