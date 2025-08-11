
using Application.Services;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStaticAnalysisService, StaticAnalysisService>();
builder.Services.AddSingleton<IDynamicAnalysisService, DynamicAnalysisService>();
builder.Services.AddSingleton<IEvolutionAnalysisService, EvolutionAnalysisService>();
builder.Services.AddSingleton<IIntegrationService, IntegrationService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/api/static", async (IStaticAnalysisService svc) => await svc.AnalyzeAsync("./src"))
   .WithName("StaticAnalysis")
   .WithOpenApi();

app.MapGet("/api/dynamic", async (IDynamicAnalysisService svc) => await svc.AnalyzeAsync("./data/logs.csv"))
   .WithName("DynamicAnalysis")
   .WithOpenApi();

app.MapGet("/api/evolution", async (IEvolutionAnalysisService svc) => await svc.AnalyzeAsync("./data/git.csv"))
   .WithName("EvolutionAnalysis")
   .WithOpenApi();

app.MapGet("/api/integration", async (
    IStaticAnalysisService s1, IDynamicAnalysisService s2, IEvolutionAnalysisService s3, IIntegrationService integ) =>
{
    var stat = await s1.AnalyzeAsync("./src");
    var dyn  = await s2.AnalyzeAsync("./data/logs.csv");
    var evo  = await s3.AnalyzeAsync("./data/git.csv");
    var mods = await integ.SuggestModulesAsync(stat, dyn, evo);
    return mods;
}).WithName("Integration")
  .WithOpenApi();

app.Run();
