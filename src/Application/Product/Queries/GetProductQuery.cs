using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Product.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Infrastructure.Common;
using ConvenienceStoreApi.Application.Common.Security;
namespace ConvenienceStoreApi.Application.Product.Queries;


[Authorize(Roles = $"{Roles.Master},{Roles.User}")]

public class GetProductQuery : IRequest<ProductResponseModel>
{
    public int Id { get; set; }
}
public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductResponseModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ProductResponseModel> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tbl_Product
            .FirstAsync(x => x.IsActive == true
            && x.PK_Product == (request.Id));

        var ProductResponseModel = _mapper.Map<ProductResponseModel>(entity);

        return ProductResponseModel;
    }
}