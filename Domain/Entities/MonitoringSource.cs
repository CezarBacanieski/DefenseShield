using DefenseShield.Domain.Exceptions;

namespace DefenseShield.Domain.Entities;

public abstract class MonitoringSource
{
    protected MonitoringSource(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidAlertDataException("Nome da fonte de monitoramento e obrigatorio.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidAlertDataException("Descricao da fonte de monitoramento e obrigatoria.");
        }

        Name = name.Trim();
        Description = description.Trim();
    }

    public string Name { get; }

    public string Description { get; }

    public abstract string GetSourceType();
}
