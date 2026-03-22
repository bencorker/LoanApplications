using System.ComponentModel;
using LoanApplications.Core;
using LoanApplications.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplications.API.Endpoints;

internal static class GetLoanApplication
{
    internal static void MapGetLoanApplication(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}",
                async ([Description("Id of the loan application to get")]Guid id, ILoanApplicationService loanApplicationService, ILogger<LoanApplicationService> logger,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        var result = await loanApplicationService.GetLoanApplication(id, cancellationToken);
                        if (result == null)
                        {
                            return Results.NotFound($"Unable to find loan application with id: {id}");
                        }
                        return Results.Ok(result);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Internal Server Error");
                        return Results.InternalServerError();
                    }
                }).WithName("GetLoanApplication")
            .WithSummary("Get a loan application by its ID.")
            .WithDescription("Retrieves a loan application from the system based on its unique identifier.")
            .Produces<LoanApplicationResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}