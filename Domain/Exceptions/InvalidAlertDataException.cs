namespace DefenseShield.Domain.Exceptions;

public sealed class InvalidAlertDataException : Exception
{
    public InvalidAlertDataException(string message)
        : base(message)
    {
    }
}
