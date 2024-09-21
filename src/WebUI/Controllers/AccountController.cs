using MediatR;
using Microsoft.AspNetCore.Mvc;
using ConvenienceStoreApi.Application.Common.Models;
using ConvenienceStoreApi.Application.Account.Commands;
using ConvenienceStoreApi.Application.Security.Models;
using ConvenienceStoreApi.Application.Security.Queries;

namespace ConvenienceStoreApi.WebUI.Controllers;

[Route("api/security/Account")]
public class AccountController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("LoginWeb")]
    public Task<AuthenticationWebModel> LoginWeb([FromBody] LoginCommand command) => _mediator.Send(command);

    [HttpPost]
    [Route("DeleteUser")]
    public Task<bool> Token([FromBody] DeleteUserCommand command) => _mediator.Send(command);

    //VALIDATE EMAIL TO REGISTER APP
    [HttpPost]
    [Route("ValidateMail")]
    public Task<bool> ValidateMail([FromBody] ValidateEmailCommand command) => _mediator.Send(command);

    [HttpPut]
    [Route("UpdatePass")]
    public Task<bool> UpdatePass([FromBody] UpdatePassCommand command) => _mediator.Send(command);

    [HttpPut]
    [Route("UpdateUser")]
    public Task<bool> UpdateUser([FromBody] UpdateUserCommand command) => _mediator.Send(command);

    [HttpGet]
    [Route("Users")]
    public Task<PaginatedList<ApplicationUserDto>> GetUsers([FromQuery] GetUsuariosQuery query) => _mediator.Send(query);
}