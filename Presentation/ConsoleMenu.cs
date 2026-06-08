using System.Globalization;
using DefenseShield.Application.Services;
using DefenseShield.Domain.Enums;
using DefenseShield.Domain.Exceptions;
using DefenseShield.Domain.ValueObjects;

namespace DefenseShield.Presentation;

public sealed class ConsoleMenu
{
    private readonly RiskAlertService _alertService;

    public ConsoleMenu(RiskAlertService alertService)
    {
        _alertService = alertService;
    }

    public void Run()
    {
        var running = true;

        while (running)
        {
            try
            {
                ConsolePrinter.PrintTitle();
                PrintOptions();
                var option = ReadRequired("Opcao");

                switch (option)
                {
                    case "1":
                        CreateAlertOption();
                        break;
                    case "2":
                        ListAlertsOption();
                        break;
                    case "3":
                        FindAlertOption();
                        break;
                    case "4":
                        UpdateAlertOption();
                        break;
                    case "5":
                        ProcessAlertOption();
                        break;
                    case "6":
                        ResolveAlertOption();
                        break;
                    case "7":
                        DeleteAlertOption();
                        break;
                    case "8":
                        ExportReportOption();
                        break;
                    case "9":
                        SimulateValidationErrorOption();
                        break;
                    case "0":
                        running = false;
                        ConsolePrinter.PrintInfo("Sistema encerrado.");
                        break;
                    default:
                        ConsolePrinter.PrintError("Opcao invalida.");
                        break;
                }
            }
            catch (AlertNotFoundException ex)
            {
                ConsolePrinter.PrintError(ex.Message);
            }
            catch (InvalidAlertDataException ex)
            {
                ConsolePrinter.PrintError(ex.Message);
            }
            catch (RepositoryException ex)
            {
                ConsolePrinter.PrintError(ex.Message);
            }
            catch (Exception ex)
            {
                ConsolePrinter.PrintError($"Erro inesperado: {ex.Message}");
            }

            if (running)
            {
                Pause();
            }
        }
    }

    private void CreateAlertOption()
    {
        ConsolePrinter.PrintInfo("Cadastro de alerta");

        var title = ReadRequired("Titulo");
        var description = ReadRequired("Descricao");
        var riskType = ReadEnum<RiskType>("Tipo de risco");
        var severity = ReadEnum<RiskSeverity>("Severidade");
        var location = ReadLocation();
        var sourceName = ReadRequired("Fonte de monitoramento");
        var detectedAt = ReadDateTime("Data de deteccao (Enter para agora)");

        var alert = _alertService.CreateAlert(title, description, riskType, severity, location, sourceName, detectedAt);

        ConsolePrinter.PrintSuccess("Alerta cadastrado com sucesso.");
        ConsolePrinter.PrintAlert(alert);
    }

    private void ListAlertsOption()
    {
        var alerts = _alertService.GetAllAlerts();

        if (alerts.Count == 0)
        {
            ConsolePrinter.PrintInfo("Nenhum alerta cadastrado.");
            return;
        }

        foreach (var alert in alerts)
        {
            ConsolePrinter.PrintAlert(alert);
        }
    }

    private void FindAlertOption()
    {
        var id = ReadGuid("ID do alerta");
        var alert = _alertService.GetAlertById(id);

        ConsolePrinter.PrintAlert(alert);
    }

    private void UpdateAlertOption()
    {
        var id = ReadGuid("ID do alerta");
        var currentAlert = _alertService.GetAlertById(id);

        ConsolePrinter.PrintInfo("Dados atuais:");
        ConsolePrinter.PrintAlert(currentAlert);
        ConsolePrinter.PrintInfo("Informe os novos dados.");

        var title = ReadRequired("Titulo");
        var description = ReadRequired("Descricao");
        var riskType = ReadEnum<RiskType>("Tipo de risco");
        var severity = ReadEnum<RiskSeverity>("Severidade");
        var location = ReadLocation();
        var sourceName = ReadRequired("Fonte de monitoramento");
        var detectedAt = ReadDateTime("Data de deteccao (Enter para agora)");

        var updatedAlert = _alertService.UpdateAlert(id, title, description, riskType, severity, location, sourceName, detectedAt);

        ConsolePrinter.PrintSuccess("Alerta atualizado com sucesso.");
        ConsolePrinter.PrintAlert(updatedAlert);
    }

    private void ProcessAlertOption()
    {
        var id = ReadGuid("ID do alerta");
        var alert = _alertService.ProcessAlert(id);

        ConsolePrinter.PrintSuccess("Alerta processado com score, prioridade e recomendacao.");
        ConsolePrinter.PrintAlert(alert);
    }

    private void ResolveAlertOption()
    {
        var id = ReadGuid("ID do alerta");
        var alert = _alertService.ResolveAlert(id);

        ConsolePrinter.PrintSuccess("Alerta resolvido com sucesso.");
        ConsolePrinter.PrintAlert(alert);
    }

    private void DeleteAlertOption()
    {
        var id = ReadGuid("ID do alerta");

        _alertService.DeleteAlert(id);

        ConsolePrinter.PrintSuccess("Alerta removido com sucesso.");
    }

    private void ExportReportOption()
    {
        Console.Write("Caminho do relatorio JSON (Enter para Data/report.json): ");
        var input = Console.ReadLine();
        var filePath = string.IsNullOrWhiteSpace(input) ? "Data/report.json" : input.Trim();

        _alertService.ExportReport(filePath);

        ConsolePrinter.PrintSuccess($"Relatorio exportado em {filePath}.");
    }

    private void SimulateValidationErrorOption()
    {
        ConsolePrinter.PrintInfo("Simulando coordenada invalida...");
        _ = new GeoCoordinate(120, 200);
    }

    private static void PrintOptions()
    {
        Console.WriteLine("1. Cadastrar alerta");
        Console.WriteLine("2. Listar alertas");
        Console.WriteLine("3. Buscar alerta por ID");
        Console.WriteLine("4. Atualizar alerta");
        Console.WriteLine("5. Processar alerta");
        Console.WriteLine("6. Resolver alerta");
        Console.WriteLine("7. Remover alerta");
        Console.WriteLine("8. Exportar relatorio JSON");
        Console.WriteLine("9. Simular erro de validacao");
        Console.WriteLine("0. Sair");
        ConsolePrinter.PrintDivider();
    }

    private static string ReadRequired(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            ConsolePrinter.PrintError($"{label} e obrigatorio.");
        }
    }

    private static Guid ReadGuid(string label)
    {
        while (true)
        {
            var input = ReadRequired(label);

            if (Guid.TryParse(input, out var id))
            {
                return id;
            }

            ConsolePrinter.PrintError("Informe um GUID valido.");
        }
    }

    private static GeoCoordinate ReadLocation()
    {
        var latitude = ReadDouble("Latitude (-90 a 90)");
        var longitude = ReadDouble("Longitude (-180 a 180)");

        return new GeoCoordinate(latitude, longitude);
    }

    private static double ReadDouble(string label)
    {
        while (true)
        {
            var input = ReadRequired(label);

            if (TryParseDouble(input, out var value))
            {
                return value;
            }

            ConsolePrinter.PrintError("Informe um numero valido.");
        }
    }

    private static bool TryParseDouble(string input, out double value)
    {
        return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
            || double.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
    }

    private static DateTime ReadDateTime(string label)
    {
        while (true)
        {
            Console.Write($"{label}: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return DateTime.Now;
            }

            if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }

            ConsolePrinter.PrintError("Informe uma data valida.");
        }
    }

    private static TEnum ReadEnum<TEnum>(string label)
        where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>();

        while (true)
        {
            ConsolePrinter.PrintInfo(label);

            for (var index = 0; index < values.Length; index++)
            {
                Console.WriteLine($"{index + 1}. {values[index]}");
            }

            var input = ReadRequired(label);

            if (int.TryParse(input, out var number) && number >= 1 && number <= values.Length)
            {
                return values[number - 1];
            }

            if (Enum.TryParse<TEnum>(input, ignoreCase: true, out var parsed) && Enum.IsDefined(parsed))
            {
                return parsed;
            }

            ConsolePrinter.PrintError("Valor invalido para o enum.");
        }
    }

    private static void Pause()
    {
        ConsolePrinter.PrintDivider();
        Console.Write("Pressione Enter para continuar...");
        Console.ReadLine();
    }
}
