using MediatR;
using ConvenienceStoreApi.Application.Common.Exceptions;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Common.Security;
using ConvenienceStoreApi.Domain.Entities;
using ConvenienceStoreApi.Infrastructure.Common;

namespace ConvenienceStoreApi.Application.Account.Commands;

[Authorize(Roles = $"{Roles.Master},{Roles.User}")]

public class UpdateUserCommand : IRequest<bool>
{
    public string UserId { get; set; }
    public string? Nombre { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UpdateUserCommandHandler(ICurrentUserService currentUserService, IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _identityService.ChangeNameUser(request.UserId, request.Nombre);
        }
        catch (Exception ex)
        {
            throw new ValidationException("Error");
        }

        return true;
    }
}