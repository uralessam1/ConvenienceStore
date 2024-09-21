using MediatR;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Common.Security;
using ConvenienceStoreApi.Application.Role.Models;
using ConvenienceStoreApi.Infrastructure.Common;

namespace ConvenienceStoreApi.Application.Role.Queries;

[Authorize(Roles = $"{Roles.Master},{Roles.User}")]
public class GetRolesQuery : IRequest<List<RoleDto>>
{
}

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public GetRolesQueryHandler(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var res = await _identityService.GetRolesAsync();

        return res;
    }
}