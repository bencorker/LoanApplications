using System.ComponentModel.DataAnnotations;
using LoanApplications.Core.Enums;

namespace LoanApplications.Core.Data;

public class LoanApplication
{
    public Guid Id { get; set; }
    [MaxLength(255)]
    public required string Name { get; set; }
    [MaxLength(255)]
    public required string Email { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal RequestedAmount { get; set; }
    public int TermMonths { get; set; }
    public LoanStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public List<DecisionLogEntry>? DecisionLogEntries { get; set; }
}
