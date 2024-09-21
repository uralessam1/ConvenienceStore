using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Application.Product.Queries;
using ConvenienceStoreApi.Application.Common.Interfaces;

namespace ConvenienceStoreApi.Application.Product.Validators;

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    private readonly IApplicationDbContext _context;

    public GetProductQueryValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Id)
            .NotNull().WithMessage("{PropertyName} must have a value")
            .GreaterThan(0).WithMessage("{PropertyName} must be a positive integer value")
            .MustAsync(ProductExist).WithMessage("{PropertyName} not found");
    }

    private async Task<bool> ProductExist(int id, CancellationToken cancellationToken)
    {
        return await _context.Tbl_Product.AnyAsync(x => x.PK_Product == id && x.IsActive);
    }
}