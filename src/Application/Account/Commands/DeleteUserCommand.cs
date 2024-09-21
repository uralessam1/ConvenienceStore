using MediatR;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Common.Security;
using ConvenienceStoreApi.Infrastructure.Common;

namespace ConvenienceStoreApi.Application.Account.Commands;

[Authorize(Roles = $"{Roles.Master},{Roles.User}")]

public class DeleteUserCommand : IRequest<bool>
{
    public string? UserId { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IIdentityService _identityService;

    public DeleteUserCommandHandler(ICurrentUserService currentUser, IIdentityService identityService)
    {
        _currentUser = currentUser;
        _identityService = identityService;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _identityService.DeleteUserLogic(request.UserId ?? _currentUser.UserId);
        return true;
    }
}