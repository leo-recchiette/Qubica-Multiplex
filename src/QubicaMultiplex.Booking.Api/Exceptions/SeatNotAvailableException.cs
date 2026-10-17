using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Exceptions;

public class SeatNotAvailableException : Exception
{
    public SeatNotAvailableException(int showId, IEnumerable<Seat> seats)
        : base($"Seats {string.Join(", ", seats.Select(seat => $"{seat.Row}{seat.Number}"))} are not available for show {showId}.")
    {
    }
}
