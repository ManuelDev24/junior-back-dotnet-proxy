using FluentValidation;
using JuniorBackDotnetProxy.Api.Dtos;

public class ProductValidator : AbstractValidator<ProductDto>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The field 'Name' is required.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("The field 'Brand' is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("The field 'Price' must be greater than 0.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("The field 'Stock' cannot be negative.");

        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("The field 'Image' is required.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("The field 'Category' is required.");
    }
}
