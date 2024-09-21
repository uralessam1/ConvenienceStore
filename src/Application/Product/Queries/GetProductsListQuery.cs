using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Common.Models;
using ConvenienceStoreApi.Application.Product.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Domain.Entities;
using ConvenienceStoreApi.Infrastructure.Common;
using ConvenienceStoreApi.Application.Common.Security;
namespace ConvenienceStoreApi.Application.Product.Queries;


[Authorize(Roles = $"{Roles.Master},{Roles.User}")]

public class GetProductsListQuery : IRequest<PaginatedList<ProductResponseModel>>
{
    public int PageSize { get; set; } = 20;
    public int PageNumber { get; set; } = 0;

    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
}
public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, PaginatedList<ProductResponseModel>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetProductsListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<ProductResponseModel>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
    {
        List<Tbl_Product> entities = await _context.Tbl_Product
            .Where(x => x.IsActive == true
             && x.Description.Contains(request.Description ?? x.Description)
             && x.Price == (request.Price ?? x.Price)
             && x.Quantity == (request.Quantity ?? x.Quantity))
            .OrderBy(x => x.Description)
            .Skip(request.PageSize * request.PageNumber)
            .Take(request.PageSize)
            .ToListAsync();

        int count = entities.Count;
        var ProductsList = _mapper.Map<List<ProductResponseModel>>(entities);

        return new PaginatedList<ProductResponseModel>(ProductsList, count, request.PageNumber, request.PageSize);
    }
}