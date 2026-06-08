using System.Text.Json;
using System.Text.Json.Serialization;
using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Exceptions;

namespace DefenseShield.Infrastructure;

public sealed class JsonReportExporter : IReportExporter
{
    public void Export(List<RiskAlert> alerts, string filePath)
    {
        try
        {
            var directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var report = new
            {
                ExportedAt = DateTime.Now,
                TotalAlerts = alerts.Count,
                Alerts = alerts
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            options.Converters.Add(new JsonStringEnumConverter());

            File.WriteAllText(filePath, JsonSerializer.Serialize(report, options));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            throw new RepositoryException("Erro ao exportar o relatorio JSON.", ex);
        }
    }
}
