
using ConvenienceStoreApi.Application.Common.Exceptions;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Product.Models;
using AutoMapper;
using MediatR;
using ConvenienceStoreApi.Domain.Entities;
using ConvenienceStoreApi.Application.Common.Security;
using ConvenienceStoreApi.Infrastructure.Common;


namespace ConvenienceStoreApi.Application.Product.Commands;


[Authorize(Roles = $"{Roles.Master},{Roles.User}")]
public class CreateProductCommand : IRequest<ProductResponseModel>
{
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
}
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponseModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ProductResponseModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Tbl_Product entity = _mapper.Map<Tbl_Product>(request);
        try
        {
            _context.Tbl_Product.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            throw new ValidationException($"Error Create Product: {ex.Message}"); 
        }           
        var result = _mapper.Map<ProductResponseModel>(entity);
        return result;
    }
  
}

