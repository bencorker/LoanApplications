using LoanApplications.BackgroundProcessor;
using LoanApplications.Core;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddLoanApplications(builder.Configuration);
builder.Services.AddSerilog(opts =>
{
    opts.MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
        .WriteTo.Console();
});
var host = builder.Build();
host.Run();