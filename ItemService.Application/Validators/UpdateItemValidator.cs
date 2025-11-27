using FluentValidation;
using ItemService.Application.DTOs;

namespace ItemService.Application.Validators
{
    /// <summary>
    /// Validator for updating an item. Ensures valid price and description.
    /// </summary>
    public class UpdateItemValidator : AbstractValidator<UpdateItemDto>
    {
        /// <summary>
        /// Sets up validation rules for UpdateItemDto.
        /// </summary>
        public UpdateItemValidator()
        {
            // Price must be positive and <= 20000 if provided
            RuleFor(x => x.Price)
                .GreaterThan(0).When(x => x.Price.HasValue)
                .WithMessage("Price must be greater than zero")
                .LessThanOrEqualTo(20000).When(x => x.Price.HasValue)
                .WithMessage("Price must be less than or equal to 20000");

            // Description max length 50 if provided
            RuleFor(x => x.Description)
                .MaximumLength(50).When(x => x.Description != null)
                .WithMessage("Description cannot exceed 50 characters");
        }
    }
}
