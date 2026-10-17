using QubicaMultiplex.Booking.Api.Exceptions;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Extensions;

public static class SeatExtensions
{
    public static IReadOnlyCollection<Seat> PickSeats(this SeatSelection selection, IEnumerable<Seat> availableSeats, int showId)
    {
        if (selection.ChosenSeats.Count > 0)
            return selection.ChosenSeats;

        if (availableSeats.Count() < selection.QuantityToBook)
            throw new NotEnoughSeatsException(showId, selection.QuantityToBook, availableSeats.Count());

        return availableSeats.Take(selection.QuantityToBook).ToList();
    }

    public static IReadOnlyCollection<Seat> EnsureAvailable(this IReadOnlyCollection<Seat> seats, IEnumerable<Seat> availableSeats, int showId)
    {
        var unavailableSeats = seats.Except(availableSeats).ToList();

        if (unavailableSeats.Count > 0)
            throw new SeatNotAvailableException(showId, unavailableSeats);

        return seats;
    }
}
