using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Enums;

namespace DefenseShield.Application.Services;

public sealed class MediumRiskRecommendationStrategy : IRiskRecommendationStrategy
{
    public RiskSeverity Severity => RiskSeverity.Medium;

    public int CalculateScore() => 50;

    public string GetPriorityLevel() => "ATTENTION";

    public string GenerateRecommendation(RiskAlert alert)
    {
        return "Acompanhar evolucao do evento e manter equipe em alerta.";
    }
}
