using FluentValidation;
using LoanApplications.Core;
using LoanApplications.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplications.API.Endpoints;

internal static class AddLoanApplication
{
    internal static void MapAddLoanApplication(this RouteGroupBuilder group)
    {
        group.MapPost("/",
            async (LoanApplicationRequest request, ILoanApplicationService loanApplicationService, ILogger<LoanApplicationService> logger,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var result = await loanApplicationService.AddLoanApplication(request, cancellationToken);
                    return Results.Created($"/api/loan-applications/{result.Id}", result);
                }
                catch (ValidationException e)
                {
                    logger.LogError(e, "Validation Error");
                    var errors = e.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors, title: "Validation Error");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Internal Server Error");
                    return Results.InternalServerError();
                }
            }).WithName("AddLoanApplication")
            .WithSummary("Add a new loan application.")
            .WithDescription("Add a new loan application.")
            .Produces<LoanApplicationResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}