using MediatR;
using Microsoft.AspNetCore.Mvc;
using ConvenienceStoreApi.Application.Role.Models;
using ConvenienceStoreApi.Application.Role.Queries;

namespace ConvenienceStoreApi.WebUI.Controllers;

[Route("api/Role")]
public class RoleController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("GetRoles")]
    public Task<List<RoleDto>> GetPermits([FromQuery] GetRolesQuery query)
    {
        return _mediator.Send(query);
    }
}