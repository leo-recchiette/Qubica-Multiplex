using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Seat>> GetBookedSeatsAsync(int showId, CancellationToken cancellationToken);

    Task<IEnumerable<BookingDetailsEntity>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken);

    Task<BookingEntity> CreateBookingAsync(BookingRequest request, CancellationToken cancellationToken);

    Task<bool> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken);
}
