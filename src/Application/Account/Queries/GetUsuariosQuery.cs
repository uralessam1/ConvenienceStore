using MediatR;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Common.Models;
using ConvenienceStoreApi.Application.Common.Security;
using ConvenienceStoreApi.Application.Security.Models;
using ConvenienceStoreApi.Infrastructure.Common;

namespace ConvenienceStoreApi.Application.Security.Queries;

[Authorize(Roles = $"{Roles.Master},{Roles.User}")]

public class GetUsuariosQuery : IRequest<PaginatedList<ApplicationUserDto>>
{
    public int PageSize { get; set; } = 20;
    public int PageNumber { get; set; } = 0;
}

public class GetUsuariosQueryHandler : IRequestHandler<GetUsuariosQuery, PaginatedList<ApplicationUserDto>>
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public GetUsuariosQueryHandler(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    public async Task<PaginatedList<ApplicationUserDto>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
    {
        var res = await _identityService.GetUsersAsync(request.PageSize, request.PageNumber);
        return res;
    }
}

/*

var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        foreach (var role in roles)
                        {
                            var isInRole = await _identityService.IsInRoleAsync(_currentUserService.UserId, role.Trim());
                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    // Must be a member of at least one role in roles
                    if (!authorized)
{
    throw new ForbiddenAccessException("Acceso restringido");
}
                }
*/