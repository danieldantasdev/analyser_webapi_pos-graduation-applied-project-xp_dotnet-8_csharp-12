
namespace Shared.Models;

public record MetricSnapshot(
    string SystemName,
    double CBO,
    double LCOM,
    double CyclomaticComplexity,
    double InheritanceDepth
);

public record RuntimeMetrics(
    string Endpoint,
    double AvgResponseMs,
    double CpuUtilizationPct,
    long Calls
);

public record EvolutionHotspot(
    string FilePath,
    int Changes,
    double ComplexityScore
);

public record ModuleSuggestion(
    string ModuleName,
    string[] Classes,
    double Cohesion,
    double Coupling
);
