using FluentValidation;
using StockMaster.Application.DTOs;

namespace StockMaster.Application.Validators
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("{PropertyName} gereklidir.")
                .NotEmpty().WithMessage("{PropertyName} boş olamaz.");
        }
    }
}