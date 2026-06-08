using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Enums;

namespace DefenseShield.Application.Services;

public sealed class LowRiskRecommendationStrategy : IRiskRecommendationStrategy
{
    public RiskSeverity Severity => RiskSeverity.Low;

    public int CalculateScore() => 25;

    public string GetPriorityLevel() => "NORMAL";

    public string GenerateRecommendation(RiskAlert alert)
    {
        return "Monitoramento periodico recomendado.";
    }
}
