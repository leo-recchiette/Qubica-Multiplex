using FluentValidation;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Validators;

public class BookingRequestValidator : AbstractValidator<BookingRequest>
{
    public BookingRequestValidator()
    {
        RuleFor(request => request.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer id must be greater than 0.");

        RuleFor(request => request.Shows)
            .NotEmpty()
            .WithMessage("At least one show must be booked.");

        RuleFor(request => request.Shows)
            .Must(shows => shows.Select(show => show.ShowId).Distinct().Count() == shows.Count)
            .WithMessage("Each show can be booked only once per request.");

        RuleForEach(request => request.Shows).SetValidator(new ShowBookingRequestValidator());
    }
}
