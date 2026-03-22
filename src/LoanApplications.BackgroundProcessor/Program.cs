using LoanApplications.BackgroundProcessor;
using LoanApplications.Core;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddLoanApplications(builder.Configuration);

var host = builder.Build();
host.Run();