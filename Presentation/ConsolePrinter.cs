using DefenseShield.Domain.Entities;

namespace DefenseShield.Presentation;

public static class ConsolePrinter
{
    public static void PrintTitle()
    {
        PrintDivider();
        Console.WriteLine("DefenseShield Orbital Intelligence");
        Console.WriteLine("Monitoramento de alertas de risco");
        PrintDivider();
    }

    public static void PrintSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[OK] {message}");
        Console.ResetColor();
    }

    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERRO] {message}");
        Console.ResetColor();
    }

    public static void PrintInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[INFO] {message}");
        Console.ResetColor();
    }

    public static void PrintAlert(RiskAlert alert)
    {
        PrintDivider();
        Console.WriteLine($"ID: {alert.Id}");
        Console.WriteLine($"Titulo: {alert.Title}");
        Console.WriteLine($"Descricao: {alert.Description}");
        Console.WriteLine($"Tipo: {alert.RiskType}");
        Console.WriteLine($"Severidade: {alert.Severity}");
        Console.WriteLine($"Status: {alert.Status}");
        Console.WriteLine($"Localizacao: {alert.Location}");
        Console.WriteLine($"Fonte: {alert.SourceName}");
        Console.WriteLine($"Detectado em: {FormatDate(alert.DetectedAt)}");
        Console.WriteLine($"Criado em: {FormatDate(alert.CreatedAt)}");
        Console.WriteLine($"Atualizado em: {FormatDate(alert.UpdatedAt)}");
        Console.WriteLine($"Processado em: {FormatNullableDate(alert.ProcessedAt)}");
        Console.WriteLine($"Resolvido em: {FormatNullableDate(alert.ResolvedAt)}");
        Console.WriteLine($"Score: {alert.RiskScore?.ToString() ?? "Nao processado"}");
        Console.WriteLine($"Prioridade: {alert.PriorityLevel ?? "Nao processado"}");
        Console.WriteLine($"Recomendacao: {alert.Recommendation ?? "Nao processado"}");
    }

    public static void PrintDivider()
    {
        Console.WriteLine(new string('-', 72));
    }

    private static string FormatNullableDate(DateTime? date)
    {
        return date.HasValue ? FormatDate(date.Value) : "Nao informado";
    }

    private static string FormatDate(DateTime date)
    {
        return date.ToString("dd/MM/yyyy HH:mm:ss");
    }
}
