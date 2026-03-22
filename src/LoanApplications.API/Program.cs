using LoanApplications.API.Endpoints;
using LoanApplications.Core;
using LoanApplications.Core.Data;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(options => { options.AddScalarTransformers(); });
builder.Services.AddLoanApplications(builder.Configuration);
builder.Services.AddSerilog(opts =>
{
    opts.MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
        .WriteTo.Console();
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LoanApplicationDbContext>();
    await db.Database.EnsureCreatedAsync();
}
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        if (httpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
        {
            diagnosticContext.Set("CorrelationId", correlationId.ToString());
        }

        if (httpContext.Request.ContentLength is > 0)
        {
            httpContext.Request.Body.Position = 0;
            using var reader = new StreamReader(httpContext.Request.Body, leaveOpen: true);
            var body = reader.ReadToEnd();
            diagnosticContext.Set("RequestBody", body);
        }
    };
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms {CorrelationId} {RequestBody}";
});
app.MapOpenApi();
app.MapScalarApiReference("/api-docs", options =>
{
    options.WithTitle("API Documentation for Loan Applications");
    options.WithTheme(ScalarTheme.Purple);
    options.ShowOperationId();
});
app.MapLoanApplicationEndpoints();

app.Run();