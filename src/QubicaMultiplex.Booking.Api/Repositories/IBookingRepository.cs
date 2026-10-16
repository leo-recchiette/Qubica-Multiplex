using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Seat>> GetBookedSeatsAsync(int showId, CancellationToken cancellationToken);

    Task<bool> DeleteBookingAsync(int id, CancellationToken cancellationToken);
}
