namespace DefenseShield.Domain.Exceptions;

public sealed class AlertNotFoundException : Exception
{
    public AlertNotFoundException(Guid id)
        : base($"Alerta com ID {id} nao foi encontrado.")
    {
    }
}
