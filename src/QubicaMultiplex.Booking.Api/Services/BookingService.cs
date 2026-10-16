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

        return showtime.Hall.GetAvailableSeats(bookedSeats).ToList();
    }

    public async Task<IEnumerable<BookingDetails>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken)
    {
        var entities = await _bookingRepository.GetCustomerBookingsAsync(customerId, cancellationToken);

        return entities
            .GroupBy(entity => new { entity.BookingId, entity.CreatedAt })
            .Select(booking => new BookingDetails
            {
                Id = booking.Key.BookingId,
                CreatedAt = booking.Key.CreatedAt,
                Shows = booking
                    .GroupBy(entity => entity.ShowId)
                    .Select(show => new BookedShow
                    {
                        ShowId = show.Key,
                        Seats = show.Select(entity => new Seat { Row = entity.Row, Number = entity.Number }).ToList()
                    })
                    .ToList()
            })
            .ToList();
    }

    public async Task<bool> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken)
        => await _bookingRepository.DeleteBookingAsync(bookingId, cancellationToken);
}
