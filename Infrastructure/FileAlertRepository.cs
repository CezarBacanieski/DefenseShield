using System.Text.Json;
using System.Text.Json.Serialization;
using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Enums;
using DefenseShield.Domain.Exceptions;
using DefenseShield.Domain.ValueObjects;

namespace DefenseShield.Infrastructure;

public sealed class FileAlertRepository : IAlertRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public FileAlertRepository(string filePath)
    {
        _filePath = filePath;
        _jsonOptions = CreateJsonOptions();
        EnsureDataStore();
    }

    public List<RiskAlert> GetAll()
    {
        return LoadAlerts();
    }

    public RiskAlert? GetById(Guid id)
    {
        return LoadAlerts().FirstOrDefault(alert => alert.Id == id);
    }

    public void Add(RiskAlert alert)
    {
        var alerts = LoadAlerts();

        alerts.Add(alert);
        SaveAlerts(alerts);
    }

    public void Update(RiskAlert alert)
    {
        var alerts = LoadAlerts();
        var index = alerts.FindIndex(item => item.Id == alert.Id);

        if (index < 0)
        {
            throw new RepositoryException($"Nao foi possivel atualizar o alerta {alert.Id} porque ele nao existe.");
        }

        alerts[index] = alert;
        SaveAlerts(alerts);
    }

    public void Delete(Guid id)
    {
        var alerts = LoadAlerts();
        var removed = alerts.RemoveAll(alert => alert.Id == id);

        if (removed == 0)
        {
            throw new RepositoryException($"Nao foi possivel remover o alerta {id} porque ele nao existe.");
        }

        SaveAlerts(alerts);
    }

    private void EnsureDataStore()
    {
        try
        {
            var directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(File.ReadAllText(_filePath)))
            {
                SaveAlerts(CreateInitialAlerts());
            }
        }
        catch (RepositoryException)
        {
            throw;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            throw new RepositoryException("Erro ao preparar o arquivo de alertas.", ex);
        }
    }

    private List<RiskAlert> LoadAlerts()
    {
        try
        {
            EnsureFileExists();
            var json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                var initialAlerts = CreateInitialAlerts();
                SaveAlerts(initialAlerts);
                return initialAlerts;
            }

            return JsonSerializer.Deserialize<List<RiskAlert>>(json, _jsonOptions) ?? [];
        }
        catch (JsonException ex)
        {
            throw new RepositoryException("Erro ao ler o JSON de alertas. Verifique se o arquivo esta valido.", ex);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new RepositoryException("Erro ao acessar o arquivo de alertas.", ex);
        }
    }

    private void SaveAlerts(List<RiskAlert> alerts)
    {
        try
        {
            var json = JsonSerializer.Serialize(alerts, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            throw new RepositoryException("Erro ao salvar o arquivo de alertas.", ex);
        }
    }

    private void EnsureFileExists()
    {
        var directory = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, string.Empty);
        }
    }

    private static List<RiskAlert> CreateInitialAlerts()
    {
        return
        [
            new RiskAlert(
                "Queimada detectada por satelite na Amazonia",
                "Alerta critico de foco de incendio identificado por monitoramento orbital.",
                RiskType.Fire,
                RiskSeverity.Critical,
                new GeoCoordinate(-3.4653, -62.2159),
                "Orbital Sensor Network",
                DateTime.Now.AddHours(-6)),

            new RiskAlert(
                "Enchente detectada por radar orbital",
                "Alerta de enchente em area urbana identificado por radar orbital.",
                RiskType.Flood,
                RiskSeverity.High,
                new GeoCoordinate(-23.5505, -46.6333),
                "Orbital Radar System",
                DateTime.Now.AddHours(-4)),

            new RiskAlert(
                "Falha em infraestrutura critica detectada por sensor",
                "Sensor IoT indicou falha em estrutura essencial que exige acompanhamento.",
                RiskType.InfrastructureFailure,
                RiskSeverity.Medium,
                new GeoCoordinate(-22.9068, -43.1729),
                "IoT Infrastructure Sensor",
                DateTime.Now.AddHours(-2))
        ];
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }
}
