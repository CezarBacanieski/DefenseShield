using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Exceptions;

namespace DefenseShield.Application.Services;

public sealed class RiskAnalysisService : IRiskAnalysisService
{
    private readonly RecommendationStrategyResolver _strategyResolver;

    public RiskAnalysisService(IEnumerable<IRiskRecommendationStrategy> strategies)
    {
        _strategyResolver = new RecommendationStrategyResolver(strategies);
    }

    public void Process(RiskAlert alert)
    {
        if (alert is null)
        {
            throw new InvalidAlertDataException("Alerta nao pode ser nulo.");
        }

        var strategy = _strategyResolver.Resolve(alert.Severity);

        alert.ProcessAnalysis(
            strategy.CalculateScore(),
            strategy.GetPriorityLevel(),
            strategy.GenerateRecommendation(alert));
    }
}
