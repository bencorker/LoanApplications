namespace LoanApplications.API.Endpoints;

internal static class LoanApplicationEndpoints
{
    internal static RouteGroupBuilder MapLoanApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/loan-applications")
            .WithTags("LoanApplications")
            .WithDescription("Endpoints for managing loan applications.");
        group.MapAddLoanApplication();
        group.MapGetLoanApplication();
        return group;
    }
}