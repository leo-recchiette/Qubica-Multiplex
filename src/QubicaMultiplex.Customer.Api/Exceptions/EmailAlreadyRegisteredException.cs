namespace QubicaMultiplex.Customer.Api.Exceptions;

public class EmailAlreadyRegisteredException : Exception
{
    public EmailAlreadyRegisteredException(string email, Exception innerException)
        : base($"Email {email} is already registered.", innerException)
    {
    }
}
