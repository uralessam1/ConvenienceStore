using MediatR;
using ConvenienceStoreApi.Application.Common.Interfaces;

namespace ConvenienceStoreApi.Application.Account.Commands;

public class ValidateEmailCommand : IRequest<bool>
{
    public string Email { get; set; }
}

public class ValidateEmailCommandHandler : IRequestHandler<ValidateEmailCommand, bool>
{
    private readonly IIdentityService _identityService;

    public ValidateEmailCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<bool> Handle(ValidateEmailCommand request, CancellationToken cancellationToken)
    {
        bool result = await _identityService.UsernameExist(request.Email);
        return result;
    }
}