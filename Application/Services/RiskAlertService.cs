using DefenseShield.Application.Interfaces;
using DefenseShield.Domain.Entities;
using DefenseShield.Domain.Enums;
using DefenseShield.Domain.Exceptions;
using DefenseShield.Domain.ValueObjects;

namespace DefenseShield.Application.Services;

public sealed class RiskAlertService
{
    private readonly IAlertRepository _repository;
    private readonly IRiskAnalysisService _analysisService;
    private readonly IReportExporter _reportExporter;

    public RiskAlertService(
        IAlertRepository repository,
        IRiskAnalysisService analysisService,
        IReportExporter reportExporter)
    {
        _repository = repository;
        _analysisService = analysisService;
        _reportExporter = reportExporter;
    }

    public RiskAlert CreateAlert(
        string title,
        string description,
        RiskType riskType,
        RiskSeverity severity,
        GeoCoordinate location,
        string sourceName,
        DateTime detectedAt)
    {
        var alert = new RiskAlert(title, description, riskType, severity, location, sourceName, detectedAt);

        _repository.Add(alert);

        return alert;
    }

    public List<RiskAlert> GetAllAlerts()
    {
        return _repository.GetAll();
    }

    public RiskAlert GetAlertById(Guid id)
    {
        return FindAlertOrThrow(id);
    }

    public RiskAlert UpdateAlert(
        Guid id,
        string title,
        string description,
        RiskType riskType,
        RiskSeverity severity,
        GeoCoordinate location,
        string sourceName,
        DateTime detectedAt)
    {
        var alert = FindAlertOrThrow(id);

        alert.Update(title, description, riskType, severity, location, sourceName, detectedAt);
        _repository.Update(alert);

        return alert;
    }

    public RiskAlert ProcessAlert(Guid id)
    {
        var alert = FindAlertOrThrow(id);

        _analysisService.Process(alert);
        _repository.Update(alert);

        return alert;
    }

    public RiskAlert ResolveAlert(Guid id)
    {
        var alert = FindAlertOrThrow(id);

        alert.Resolve();
        _repository.Update(alert);

        return alert;
    }

    public void DeleteAlert(Guid id)
    {
        _ = FindAlertOrThrow(id);
        _repository.Delete(id);
    }

    public void ExportReport(string filePath)
    {
        _reportExporter.Export(_repository.GetAll(), filePath);
    }

    private RiskAlert FindAlertOrThrow(Guid id)
    {
        return _repository.GetById(id) ?? throw new AlertNotFoundException(id);
    }
}
