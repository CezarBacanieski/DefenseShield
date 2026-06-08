using System.Text.Json.Serialization;
using DefenseShield.Domain.Enums;
using DefenseShield.Domain.Exceptions;
using DefenseShield.Domain.ValueObjects;

namespace DefenseShield.Domain.Entities;

public partial class RiskAlert : BaseEntity
{
    public RiskAlert()
    {
        Title = string.Empty;
        Description = string.Empty;
        SourceName = string.Empty;
        Location = new GeoCoordinate(0, 0);
        Status = AlertStatus.Open;
        DetectedAt = DateTime.Now;
    }

    public RiskAlert(
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
        Status = AlertStatus.Open;
        Location = location;
        SourceName = NormalizeRequiredText(sourceName, "Fonte");
        DetectedAt = detectedAt;

        Validate();
    }

    [JsonInclude]
    public string Title { get; private set; }

    [JsonInclude]
    public string Description { get; private set; }

    [JsonInclude]
    public RiskType RiskType { get; private set; }

    [JsonInclude]
    public RiskSeverity Severity { get; private set; }

    [JsonInclude]
    public AlertStatus Status { get; private set; }

    [JsonInclude]
    public GeoCoordinate Location { get; private set; }

    [JsonInclude]
    public string SourceName { get; private set; }

    [JsonInclude]
    public DateTime DetectedAt { get; private set; }

    [JsonInclude]
    public DateTime? ProcessedAt { get; private set; }

    [JsonInclude]
    public DateTime? ResolvedAt { get; private set; }

    [JsonInclude]
    public string? Recommendation { get; private set; }

    [JsonInclude]
    public int? RiskScore { get; private set; }

    [JsonInclude]
    public string? PriorityLevel { get; private set; }

    private static string NormalizeRequiredText(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidAlertDataException($"{fieldName} e obrigatorio.");
        }

        return value.Trim();
    }
}
