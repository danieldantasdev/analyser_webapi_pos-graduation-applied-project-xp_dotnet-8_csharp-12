
using Shared.Models;

namespace Application.Services;

public interface IStaticAnalysisService
{
    Task<MetricSnapshot[]> AnalyzeAsync(string sourceRoot);
}

public interface IDynamicAnalysisService
{
    Task<RuntimeMetrics[]> AnalyzeAsync(string logsCsvPath);
}

public interface IEvolutionAnalysisService
{
    Task<EvolutionHotspot[]> AnalyzeAsync(string gitHistoryCsvPath);
}

public interface IIntegrationService
{
    Task<ModuleSuggestion[]> SuggestModulesAsync(
        MetricSnapshot[] statics,
        RuntimeMetrics[] runtime,
        EvolutionHotspot[] evolution);
}

// --- Fake implementations with plausible calculations ---
public class StaticAnalysisService : IStaticAnalysisService
{
    public async Task<MetricSnapshot[]> AnalyzeAsync(string sourceRoot)
    {
        // In a real impl, parse C# with Roslyn. Here we simulate scanning files.
        await Task.Delay(50);
        var rnd = new Random(42);
        string[] systems = new[] { "SistemaA", "SistemaB", "SistemaC" };
        return systems.Select(s => new MetricSnapshot(
            s,
            CBO: Math.Round(8 + rnd.NextDouble()*6, 1),           // 8..14
            LCOM: Math.Round(0.55 + rnd.NextDouble()*0.3, 2),      // 0.55..0.85
            CyclomaticComplexity: Math.Round(6 + rnd.NextDouble()*6, 1), // 6..12
            InheritanceDepth: Math.Round(2 + rnd.NextDouble()*3, 1) // 2..5
        )).ToArray();
    }
}

public class DynamicAnalysisService : IDynamicAnalysisService
{
    public async Task<RuntimeMetrics[]> AnalyzeAsync(string logsCsvPath)
    {
        await Task.Delay(50);
        // Fake endpoints
        var endpoints = new[]{"/api/orders","/api/payments","/api/users","/api/reviews"};
        var rnd = new Random(7);
        return endpoints.Select(e => new RuntimeMetrics(
            e,
            AvgResponseMs: Math.Round(150 + rnd.NextDouble()*300, 1),
            CpuUtilizationPct: Math.Round(10 + rnd.NextDouble()*70, 1),
            Calls: rnd.Next(10000, 1000000)
        )).ToArray();
    }
}

public class EvolutionAnalysisService : IEvolutionAnalysisService
{
    public async Task<EvolutionHotspot[]> AnalyzeAsync(string gitHistoryCsvPath)
    {
        await Task.Delay(50);
        var rnd = new Random(99);
        string[] files = new[] {
            "Services/OrderService.cs","Controllers/OrderController.cs","Data/OrderRepo.cs",
            "Services/PaymentService.cs","Controllers/PaymentController.cs",
            "Core/Mapping/Mapper.cs","Legacy/Monolith/GodClass.cs"
        };
        return files.Select(f => new EvolutionHotspot(
            f,
            Changes: rnd.Next(20, 300),
            ComplexityScore: Math.Round(5 + rnd.NextDouble()*20, 2)
        )).OrderByDescending(h=>h.Changes).ToArray();
    }
}

public class IntegrationService : IIntegrationService
{
    public async Task<ModuleSuggestion[]> SuggestModulesAsync(
        MetricSnapshot[] statics, RuntimeMetrics[] runtime, EvolutionHotspot[] evolution)
    {
        await Task.Delay(50);
        // Simplified: create modules based on filename prefixes and hotspots
        var groups = evolution.GroupBy(h => h.FilePath.Split('/')[0]);
        var rnd = new Random(3);
        var mods = new List<ModuleSuggestion>();
        int i=1;
        foreach (var g in groups)
        {
            var classes = g.Select(x => x.FilePath.Replace("/", ".").Replace(".cs","")).ToArray();
            mods.Add(new ModuleSuggestion(
                $"M{i++}_{g.Key}",
                classes,
                Cohesion: Math.Round(0.65 + rnd.NextDouble()*0.2, 2),
                Coupling: Math.Round(0.2 + rnd.NextDouble()*0.2, 2)
            ));
        }
        return mods.ToArray();
    }
}
