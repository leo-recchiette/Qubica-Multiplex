namespace QubicaMultiplex.Booking.Api.Exceptions;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(int customerId) : base($"Customer {customerId} does not exist.")
    {
    }
}
