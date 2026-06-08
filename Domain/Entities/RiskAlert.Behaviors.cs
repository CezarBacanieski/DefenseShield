using DefenseShield.Domain.Enums;
using DefenseShield.Domain.Exceptions;
using DefenseShield.Domain.ValueObjects;

namespace DefenseShield.Domain.Entities;

public partial class RiskAlert
{
    public void Update(
        string title,
        string description,
        RiskType riskType,
        RiskSeverity severity,
        GeoCoordinate location,
        string sourceName,
        DateTime detectedAt)
    {
        Title = NormalizeRequiredText(title, "Titulo");
        Description = NormalizeRequiredText(description, "Descricao");
        RiskType = riskType;
        Severity = severity;
        Location = location;
        SourceName = NormalizeRequiredText(sourceName, "Fonte");
        DetectedAt = detectedAt;

        Validate();
        Touch();
    }

    public void ProcessAnalysis(int riskScore, string priorityLevel, string recommendation)
    {
        if (riskScore is < 0 or > 100)
        {
            throw new InvalidAlertDataException("Score de risco deve estar entre 0 e 100.");
        }

        RiskScore = riskScore;
        PriorityLevel = NormalizeRequiredText(priorityLevel, "Nivel de prioridade");
        Recommendation = NormalizeRequiredText(recommendation, "Recomendacao");
        ProcessedAt = DateTime.Now;
        Status = AlertStatus.InAnalysis;

        Validate();
        Touch();
    }

    public void Resolve()
    {
        Status = AlertStatus.Resolved;
        ResolvedAt = DateTime.Now;

        Validate();
        Touch();
    }

    public void Validate()
    {
        _ = NormalizeRequiredText(Title, "Titulo");
        _ = NormalizeRequiredText(Description, "Descricao");
        _ = NormalizeRequiredText(SourceName, "Fonte");

        if (!Enum.IsDefined(RiskType))
        {
            throw new InvalidAlertDataException("Tipo de risco invalido.");
        }

        if (!Enum.IsDefined(Severity))
        {
            throw new InvalidAlertDataException("Severidade invalida.");
        }

        if (!Enum.IsDefined(Status))
        {
            throw new InvalidAlertDataException("Status invalido.");
        }

        if (DetectedAt > DateTime.Now.AddMinutes(1))
        {
            throw new InvalidAlertDataException("Data de deteccao nao pode estar no futuro.");
        }

        if (RiskScore is < 0 or > 100)
        {
            throw new InvalidAlertDataException("Score de risco deve estar entre 0 e 100.");
        }

        if (Status == AlertStatus.Resolved && ResolvedAt is null)
        {
            throw new InvalidAlertDataException("Alertas resolvidos precisam de data de resolucao.");
        }
    }
}
