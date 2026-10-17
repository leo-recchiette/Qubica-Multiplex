using FluentValidation;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Validators;

public class ShowBookingRequestValidator : AbstractValidator<ShowBookingRequest>
{
    public ShowBookingRequestValidator()
    {
        RuleFor(show => show.ShowId)
            .GreaterThan(0)
            .WithMessage("Show id must be greater than 0.");

        RuleFor(show => show.Seats).SetValidator(new SeatSelectionValidator());
    }
}
