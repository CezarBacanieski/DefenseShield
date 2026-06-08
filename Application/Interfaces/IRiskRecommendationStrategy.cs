using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Enums;

namespace DefenseShield.Application.Interfaces;

public interface IRiskRecommendationStrategy
{
    RiskSeverity Severity { get; }

    int CalculateScore();

    string GetPriorityLevel();

    string GenerateRecommendation(RiskAlert alert);
}
