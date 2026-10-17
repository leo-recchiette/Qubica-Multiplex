using FluentValidation;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Validators;

public class SeatSelectionValidator : AbstractValidator<SeatSelection>
{
    public SeatSelectionValidator()
    {
        RuleFor(selection => selection.QuantityToBook)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Quantity to book must be at least 1.");

        RuleFor(selection => selection.ChosenSeats)
            .Must((selection, chosenSeats) => chosenSeats.Count == selection.QuantityToBook)
            .When(selection => selection.ChosenSeats.Count > 0)
            .WithMessage("The number of chosen seats must match the quantity to book.");

        RuleFor(selection => selection.ChosenSeats)
            .Must(chosenSeats => chosenSeats.Distinct().Count() == chosenSeats.Count)
            .WithMessage("Chosen seats must not contain duplicates.");

        RuleForEach(selection => selection.ChosenSeats).SetValidator(new SeatValidator());
    }
}
