using LoanApplications.API.Endpoints;
using LoanApplications.Core;
using LoanApplications.Core.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(options => { options.AddScalarTransformers(); });
builder.Services.AddLoanApplications(builder.Configuration);

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