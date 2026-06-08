using DefenseShield.Domain.Entities;

namespace DefenseShield.Application.Interfaces;

public interface IReportExporter
{
    void Export(List<RiskAlert> alerts, string filePath);
}
