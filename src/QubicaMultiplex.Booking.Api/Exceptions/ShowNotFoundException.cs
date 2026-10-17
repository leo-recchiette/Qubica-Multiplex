namespace QubicaMultiplex.Booking.Api.Exceptions;

public class ShowNotFoundException : Exception
{
    public ShowNotFoundException(int showId) : base($"Show {showId} does not exist.")
    {
    }
}
