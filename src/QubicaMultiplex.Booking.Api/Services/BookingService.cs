using System.Net;
using QubicaMultiplex.Booking.Api.Clients;
using QubicaMultiplex.Booking.Api.Extensions;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Repositories;

namespace QubicaMultiplex.Booking.Api.Services;

public class BookingService : IBookingService
{
    private readonly ICatalogClient _catalogClient;
    private readonly IBookingRepository _bookingRepository;

    public BookingService(ICatalogClient catalogClient, IBookingRepository bookingRepository)
    {
        _catalogClient = catalogClient;
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<Seat>?> GetAvailableSeatsAsync(int showId, CancellationToken cancellationToken)
    {
        using var response = await _catalogClient.GetShowtimeDetailsAsync(showId, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
            throw response.Error!;

        var showtime = response.Content!;

        var bookedSeats = await _bookingRepository.GetBookedSeatsAsync(showId, cancellationToken);

        return showtime.Hall.GetAllSeats().Except(bookedSeats).ToList();
    }
}
