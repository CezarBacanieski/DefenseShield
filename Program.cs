using DefenseShield.Application.Interfaces;
using DefenseShield.Application.Services;
using DefenseShield.Infrastructure;
using DefenseShield.Presentation;

var strategies = new List<IRiskRecommendationStrategy>
{
    new LowRiskRecommendationStrategy(),
    new MediumRiskRecommendationStrategy(),
    new HighRiskRecommendationStrategy(),
    new CriticalRiskRecommendationStrategy()
};

var repository = new FileAlertRepository("Data/alerts.json");
var exporter = new JsonReportExporter();
var analysisService = new RiskAnalysisService(strategies);
var alertService = new RiskAlertService(repository, analysisService, exporter);
var menu = new ConsoleMenu(alertService);

menu.Run();
