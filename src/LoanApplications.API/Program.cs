using LoanApplications.API.Endpoints;
using LoanApplications.Core;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(options => { options.AddScalarTransformers(); });
builder.Services.AddTransient<ILoanApplicationService, LoanApplicationService>();

var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference("/api-docs", options =>
{
    options.WithTitle("API Documentation for Loan Applications");
    options.WithTheme(ScalarTheme.Purple);
    options.ShowOperationId();
});
app.MapLoanApplicationEndpoints();

app.Run();