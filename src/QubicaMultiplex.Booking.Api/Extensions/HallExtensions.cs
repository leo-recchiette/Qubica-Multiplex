using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Extensions;

public static class HallExtensions
{
    public static IEnumerable<Seat> GetAvailableSeats(this Hall hall, IEnumerable<Seat> bookedSeats)
        => hall.GetAllSeats().Except(bookedSeats);

    private static IEnumerable<Seat> GetAllSeats(this Hall hall)
    {
        var seats = new List<Seat>();

        for (var row = 'A'; row <= hall.LastRow[0]; row++)
        {
            for (var number = 1; number <= hall.SeatsPerRow; number++)
            {
                seats.Add(new Seat { Row = row.ToString(), Number = number });
            }
        }

        return seats;
    }
}
