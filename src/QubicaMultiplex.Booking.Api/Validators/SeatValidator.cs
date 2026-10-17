using FluentValidation;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Validators;

public class SeatValidator : AbstractValidator<Seat>
{
    public SeatValidator()
    {
        RuleFor(seat => seat.Row)
            .Matches("^[A-Z]$")
            .WithMessage("Seat row must be a single letter from A to Z.");

        RuleFor(seat => seat.Number)
            .GreaterThan(0)
            .WithMessage("Seat number must be greater than 0.");
    }
}
