namespace QubicaMultiplex.Booking.Api.Exceptions;

public class NotEnoughSeatsException : Exception
{
    public NotEnoughSeatsException(int showId, int requestedSeats, int availableSeats)
        : base($"Show {showId} has only {availableSeats} available seats, {requestedSeats} requested.")
    {
    }
}
