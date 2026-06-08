using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Enums;

namespace DefenseShield.Application.Services;

public sealed class HighRiskRecommendationStrategy : IRiskRecommendationStrategy
{
    public RiskSeverity Severity => RiskSeverity.High;

    public int CalculateScore() => 75;

    public string GetPriorityLevel() => "URGENT";

    public string GenerateRecommendation(RiskAlert alert)
    {
        return "Acionar equipe tecnica e intensificar monitoramento por satelite.";
    }
}
