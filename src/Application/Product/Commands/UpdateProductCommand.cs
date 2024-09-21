using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Product.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ConvenienceStoreApi.Application.Common.Exceptions;
using ConvenienceStoreApi.Domain.Entities;
using ConvenienceStoreApi.Infrastructure.Common;
using ConvenienceStoreApi.Application.Common.Security;
namespace ConvenienceStoreApi.Application.Product.Commands;


[Authorize(Roles = $"{Roles.Master},{Roles.User}")]

public class UpdateProductCommand : IRequest<ProductResponseModel>
{
    public int ProductId { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
}
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductResponseModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ProductResponseModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var result = new ProductResponseModel();

        Tbl_Product entity = await _context.Tbl_Product
            .FirstAsync(x => x.PK_Product == request.ProductId && x.IsActive);
       
        if (entity != null)
        {
            entity.Price = request.Price ?? entity.Price;
            entity.Description = request.Description ?? entity.Description;
            entity.Quantity = request.Quantity ?? entity.Quantity;
            try
            {
                _context.Tbl_Product.Update(entity);
                if (await _context.SaveChangesAsync(cancellationToken) > 0)
                {
                    result = _mapper.Map<ProductResponseModel>(entity);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }
        else
        {
            throw new NotFoundException($"Not found Product ID: {request.ProductId}");
        }
    }

}