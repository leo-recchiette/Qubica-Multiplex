using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Services.Interfaces;

public interface ISeatService
{
    Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int showId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<BookedShow>> AssignSeatsAsync(IEnumerable<ShowBookingRequest> shows, CancellationToken cancellationToken);
}
