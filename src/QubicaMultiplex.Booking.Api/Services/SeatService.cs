using QubicaMultiplex.Booking.Api.Clients;
using QubicaMultiplex.Booking.Api.Exceptions;
using QubicaMultiplex.Booking.Api.Extensions;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Repositories.Interfaces;
using QubicaMultiplex.Booking.Api.Services.Interfaces;

namespace QubicaMultiplex.Booking.Api.Services;

public class SeatService : ISeatService
{
    private readonly ICatalogClient _catalogClient;
    private readonly IBookingRepository _bookingRepository;

    public SeatService(ICatalogClient catalogClient, IBookingRepository bookingRepository)
    {
        _catalogClient = catalogClient;
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<Seat>> GetAvailableSeatsAsync(int showId, CancellationToken cancellationToken)
    {
        using var response = await _catalogClient.GetShowtimeDetailsAsync(showId, cancellationToken);

        var showtime = response.GetShowtimeOrThrow(showId);

        var bookedSeats = await _bookingRepository.GetBookedSeatsAsync(showId, cancellationToken);

        return showtime.Hall.GetAvailableSeats(bookedSeats).ToList();
    }

    public async Task<List<BookedShow>> AssignSeatsAsync(List<ShowBookingRequest> shows, CancellationToken cancellationToken)
    {
        var bookedShows = new List<BookedShow>();

        foreach (var show in shows)
        {
            var availableSeats = await GetAvailableSeatsAsync(show.ShowId, cancellationToken);

            var seats = show.Seats.ChosenSeats;

            if (seats.Count == 0)
            {
                if (availableSeats.Count() < show.Seats.QuantityToBook)
                    throw new NotEnoughSeatsException(show.ShowId, show.Seats.QuantityToBook, availableSeats.Count());

                seats = availableSeats.Take(show.Seats.QuantityToBook).ToList();
            }

            bookedShows.Add(new BookedShow { ShowId = show.ShowId, Seats = seats });
        }

        return bookedShows;
    }
}
