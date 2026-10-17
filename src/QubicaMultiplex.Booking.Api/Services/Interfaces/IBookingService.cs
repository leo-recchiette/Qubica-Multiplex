using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Services.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingDetails>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken);

    Task<BookingDetails> CreateBookingAsync(BookingRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken);

    Task<bool> DeleteBookedShowAsync(int bookingId, int showId, CancellationToken cancellationToken);
}
