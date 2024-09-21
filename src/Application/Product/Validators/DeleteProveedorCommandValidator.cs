using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Application.Product.Commands;
using ConvenienceStoreApi.Application.Common.Interfaces;

namespace ConvenienceStoreApi.Application.Product.Validators;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.ProductId)
            .NotNull().WithMessage("{PropertyName} must have a value")
            .GreaterThan(0).WithMessage("{PropertyName} must be a positive integer value")
            .MustAsync(ProductExist).WithMessage("{PropertyName} not found");
    }

    private async Task<bool> ProductExist(int id, CancellationToken cancellationToken)
    {
        return await _context.Tbl_Product.AnyAsync(x => x.PK_Product == id && x.IsActive);
    }

}