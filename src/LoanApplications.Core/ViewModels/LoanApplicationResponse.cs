using System.Text.Json.Serialization;
using LoanApplications.Core.Enums;

namespace LoanApplications.Core.ViewModels;

public record LoanApplicationResponse
{
    public required Guid Id { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required LoanStatus Status { get; set; }
    public required DateTime CreatedAt { get; set; }
}