using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Application.Product.Commands;
using ConvenienceStoreApi.Application.Common.Interfaces;
using System.Text.RegularExpressions;

namespace ConvenienceStoreApi.Application.Product.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.ProductId)
          .NotNull().WithMessage("{PropertyName} must have a value")
          .GreaterThan(0).WithMessage("{PropertyName} must be a positive integer value")
          .MustAsync(ProductExist).WithMessage("{PropertyName} not found");

        RuleFor(v => v.Price)
            .GreaterThan(0).When(v => v.Price.HasValue).WithMessage("{PropertyName} must be a positive integer value");

        RuleFor(v => v.Description)
            .MaximumLength(200).When(v => !string.IsNullOrEmpty(v.Description)).WithMessage("{PropertyName} max length 200");

        RuleFor(v => v.Quantity)
            .GreaterThan(-1).When(v => v.Quantity.HasValue).WithMessage("{PropertyName} must be a positive integer value or zero");
    }

    private async Task<bool> ProductExist(int id, CancellationToken cancellationToken)
    {
        return await _context.Tbl_Product.AnyAsync(x => x.PK_Product == id && x.IsActive);
    }
}