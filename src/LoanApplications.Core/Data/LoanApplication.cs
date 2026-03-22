namespace LoanApplications.Core.Data;

public class LoanApplication
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int MonthlyIncome { get; set; }
    public int RequestedAmount { get; set; }
    public int TermMonths { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
