namespace QubicaMultiplex.Booking.Api.Exceptions;

public class SeatAlreadyBookedException : Exception
{
    public SeatAlreadyBookedException(Exception innerException)
        : base("One or more seats are already booked.", innerException)
    {
    }
}
