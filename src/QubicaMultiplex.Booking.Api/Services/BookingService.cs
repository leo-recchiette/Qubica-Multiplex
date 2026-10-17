using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Repositories.Interfaces;
using QubicaMultiplex.Booking.Api.Services.Interfaces;

namespace QubicaMultiplex.Booking.Api.Services;

public class BookingService : IBookingService
{
    private readonly ISeatService _seatService;
    private readonly IBookingRepository _bookingRepository;

    public BookingService(ISeatService seatService, IBookingRepository bookingRepository)
    {
        _seatService = seatService;
        _bookingRepository = bookingRepository;
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

    public async Task<BookingDetails> CreateBookingAsync(BookingRequest request, CancellationToken cancellationToken)
    {
        var bookedShows = await _seatService.AssignSeatsAsync(request.Shows, cancellationToken);

        var booking = await _bookingRepository.CreateBookingAsync(request.Customer, bookedShows, cancellationToken);

        return new BookingDetails
        {
            Id = booking.Id,
            CreatedAt = booking.CreatedAt,
            Shows = bookedShows
        };
    }

    public async Task<bool> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken)
        => await _bookingRepository.DeleteBookingAsync(bookingId, cancellationToken);

    public async Task<bool> DeleteBookedShowAsync(int bookingId, int showId, CancellationToken cancellationToken)
        => await _bookingRepository.DeleteBookedShowAsync(bookingId, showId, cancellationToken);
}
