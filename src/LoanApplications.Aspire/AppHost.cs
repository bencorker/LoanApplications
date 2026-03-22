using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<LoanApplications_API>("api").WithUrl("/api-docs", "Scalar API Docs");
builder.AddProject<LoanApplications_BackgroundProcessor>("background-processor").WithExplicitStart();
builder.Build().Run();