using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Services;

public interface IBookingService
{
    Task<IEnumerable<Seat>?> GetAvailableSeatsAsync(int showId, CancellationToken cancellationToken);

    Task<IEnumerable<BookingDetails>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken);

    Task<bool> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken);
}
