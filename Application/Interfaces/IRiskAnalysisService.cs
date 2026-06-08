using DefenseShield.Domain.Entities;

namespace DefenseShield.Application.Interfaces;

public interface IRiskAnalysisService
{
    void Process(RiskAlert alert);
}
