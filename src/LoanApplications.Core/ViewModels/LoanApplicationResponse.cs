namespace LoanApplications.Core.ViewModels;

public record LoanApplicationResponse
{
    public required Guid Id { get; set; }
    public required string Status { get; set; }
    public required DateTime CreatedAt { get; set; }
}