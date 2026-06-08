using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Enums;
using DefenseShield.Domain.Exceptions;

namespace DefenseShield.Application.Services;

public sealed class RecommendationStrategyResolver
{
    private readonly IReadOnlyCollection<IRiskRecommendationStrategy> _strategies;

    public RecommendationStrategyResolver(IEnumerable<IRiskRecommendationStrategy> strategies)
    {
        _strategies = strategies?.ToList() ?? throw new InvalidAlertDataException("A lista de estrategias nao pode ser nula.");

        if (_strategies.Count == 0)
        {
            throw new InvalidAlertDataException("Cadastre ao menos uma estrategia de recomendacao.");
        }
    }

    public IRiskRecommendationStrategy Resolve(RiskSeverity severity)
    {
        var strategy = _strategies.FirstOrDefault(item => item.Severity == severity);

        return strategy ?? throw new InvalidAlertDataException($"Nenhuma estrategia encontrada para a severidade {severity}.");
    }
}
