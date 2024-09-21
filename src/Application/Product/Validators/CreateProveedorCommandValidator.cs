using FluentValidation;
using ConvenienceStoreApi.Application.Product.Commands;

namespace ConvenienceStoreApi.Application.Product.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(v => v.Price)
            .NotNull().WithMessage("{PropertyName} must have a value")
            .NotEmpty().WithMessage("{PropertyName} must have a value")
            .GreaterThan(0).WithMessage("{PropertyName} must be a positive integer value");
        RuleFor(v => v.Description)
            .NotNull().WithMessage("{PropertyName} must have a value")
            .NotEmpty().WithMessage("{PropertyName} must have a value")
            .MaximumLength(200).WithMessage("{PropertyName} max length 200");
        RuleFor(v => v.Quantity)
            .NotNull().WithMessage("{PropertyName} must have a value")
            .NotEmpty().WithMessage("{PropertyName} must have a value")
            .GreaterThan(-1).WithMessage("{PropertyName} must be a positive integer value or zero");      
    }
}