using ConvenienceStoreApi.Application.Common.Models;
using ConvenienceStoreApi.Application.Product.Commands;
using ConvenienceStoreApi.Application.Product.Models;
using ConvenienceStoreApi.Application.Product.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
namespace ConvenienceStoreApi.WebUI.Controllers;
[Route("api/Product")]
public class ProductController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    [Route("GetProduct")]
    public  Task<ProductResponseModel> GetProductsByFilter([FromQuery] GetProductQuery query)
        =>  _mediator.Send(query);
    [HttpGet]
    [Route("GetProductsList")]
    public  Task<PaginatedList<ProductResponseModel>> GetProductsList([FromQuery] GetProductsListQuery query)
        =>  _mediator.Send(query);
    [HttpPost]
    [Route("CreateProduct")]
    public  Task<ProductResponseModel> CreateProduct([FromBody] CreateProductCommand command)
        =>  _mediator.Send(command);
    [HttpPut]
    [Route("UpdateProduct")]
    public  Task<ProductResponseModel> UpdateProduct([FromBody] UpdateProductCommand command)
        =>  _mediator.Send(command);
    [HttpPatch]
    [Route("DeleteProduct")]
    public  Task<ProductResponseModel> DeleteProduct([FromBody] DeleteProductCommand command)
        =>  _mediator.Send(command);
}