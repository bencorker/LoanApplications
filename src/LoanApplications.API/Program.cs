using FluentValidation;
using LoanApplications.API.Endpoints;
using LoanApplications.Core;
using LoanApplications.Core.Data;
using LoanApplications.Core.ViewModels;
using LoanApplications.Core.Validators;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(options => { options.AddScalarTransformers(); });
builder.Services.AddDbContext<LoanApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=loanapplications.db"));
builder.Services.AddSingleton<IValidator<LoanApplicationRequest>, LoanApplicationRequestValidator>();
builder.Services.AddTransient<ILoanApplicationService, LoanApplicationService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LoanApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
}
app.MapOpenApi();
app.MapScalarApiReference("/api-docs", options =>
{
    options.WithTitle("API Documentation for Loan Applications");
    options.WithTheme(ScalarTheme.Purple);
    options.ShowOperationId();
});
app.MapLoanApplicationEndpoints();

app.Run();