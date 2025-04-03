using FluentValidation;
using AdTechAPI.Models.DTOs;

namespace AdTechAPI.Validation
{

    public class CreatePlacementRequestValidator : AbstractValidator<CreatePlacementRequest>
    {
        public CreatePlacementRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.PublisherId)
                .GreaterThan(0).WithMessage("PublisherId is required.");

            RuleFor(x => x.TrafficSourceId)
                .GreaterThan(0).WithMessage("TrafficSourceId is required.");

            RuleFor(x => x.Verticals)
                .NotEmpty().WithMessage("At least one vertical must be selected.");
        }
    }
}