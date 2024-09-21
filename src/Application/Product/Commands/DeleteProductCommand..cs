using ConvenienceStoreApi.Application.Common.Exceptions;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Product.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Infrastructure.Common;
using ConvenienceStoreApi.Application.Common.Security;
namespace ConvenienceStoreApi.Application.Product.Commands;


[Authorize(Roles = $"{Roles.Master},{Roles.User}")]

public class DeleteProductCommand : IRequest<ProductResponseModel>
{    public int ProductId { get; set; }
}
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ProductResponseModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public DeleteProductCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ProductResponseModel> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var result = new ProductResponseModel();
        var entity = await _context.Tbl_Product.FirstAsync(x => x.PK_Product == request.ProductId && x.IsActive);
        if (entity == null) 
            throw new NotFoundException("Product not found"); 
        entity.IsActive = false;
        try
        {
            _context.Tbl_Product.Update(entity);
            if (await _context.SaveChangesAsync(cancellationToken) > 0)
                result = _mapper.Map<ProductResponseModel>(entity);
            
            return result;
        }
        catch (Exception ex)
        {
            throw new Common.Exceptions.ValidationException($"Error Delete Product:{ex.Message}");
        }
    }
}