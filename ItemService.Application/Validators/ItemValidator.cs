using FluentValidation;
using ItemService.Application.DTOs;

namespace ItemService.Application.Validators
{
    /// <summary>
    /// Validator for creating an item. Ensures required fields and valid values.
    /// </summary>
    public class ItemValidator : AbstractValidator<CreateItemDto>
    {
        /// <summary>
        /// Sets up validation rules for CreateItemDto.
        /// </summary>
        public ItemValidator()
        {
            // Name is required
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            // Price must be positive and <= 20000
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero")
                .LessThanOrEqualTo(20000).WithMessage("Price must be less than or equal to 20000");

            // Description max length 50
            RuleFor(x => x.Description)
                .MaximumLength(50).WithMessage("Description cannot exceed 50 characters");
        }
    }
}
