using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<IEnumerable<Seat>> GetBookedSeatsAsync(int showId, CancellationToken cancellationToken);

    Task<IEnumerable<BookingDetailsEntity>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken);

    Task<BookingEntity> CreateBookingAsync(int customerId, IEnumerable<BookedShow> shows, CancellationToken cancellationToken);

    Task<bool> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken);

    Task<bool> DeleteBookedShowAsync(int bookingId, int showId, CancellationToken cancellationToken);
}
