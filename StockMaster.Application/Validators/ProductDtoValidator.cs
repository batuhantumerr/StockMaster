using FluentValidation;
using StockMaster.Application.DTOs;

namespace StockMaster.Application.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("{PropertyName} gereklidir.")
                .NotEmpty().WithMessage("{PropertyName} boş olamaz.");

            RuleFor(x => x.Price)
                .InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} 0'dan büyük olmalıdır.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(0, int.MaxValue).WithMessage("{PropertyName} eksi olamaz.");

            RuleFor(x => x.CategoryId)
                .InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} seçilmelidir.");
        }
    }
}