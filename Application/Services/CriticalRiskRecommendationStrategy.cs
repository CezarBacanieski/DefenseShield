using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Enums;

namespace DefenseShield.Application.Services;

public sealed class CriticalRiskRecommendationStrategy : IRiskRecommendationStrategy
{
    public RiskSeverity Severity => RiskSeverity.Critical;

    public int CalculateScore() => 100;

    public string GetPriorityLevel() => "EMERGENCY";

    public string GenerateRecommendation(RiskAlert alert)
    {
        return "Acionar resposta imediata, monitoramento continuo e comunicacao com autoridades responsaveis.";
    }
}
