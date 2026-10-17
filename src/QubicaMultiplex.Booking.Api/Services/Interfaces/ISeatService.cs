using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Services.Interfaces;

public interface ISeatService
{
    Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int showId, CancellationToken cancellationToken);

    Task<List<BookedShow>> AssignSeatsAsync(List<ShowBookingRequest> shows, CancellationToken cancellationToken);
}
