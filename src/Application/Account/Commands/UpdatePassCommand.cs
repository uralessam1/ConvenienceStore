using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using ConvenienceStoreApi.Application.Common.Exceptions;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Common.Security;
using ConvenienceStoreApi.Infrastructure.Common;

namespace ConvenienceStoreApi.Application.Account.Commands;

[Authorize(Roles = $"{Roles.Master},{Roles.User}")]
public class UpdatePassCommand : IRequest<bool>
{
    [Required]
    public string? ActualPassword { get; set; }

    [Required]
    [RegularExpression(@"^(?=\w*\d)(?=\w*[A-Z])(?=\w*[a-z])\S{8,16}$")]
    public string? NewPassword { get; set; }

    [DefaultValue(true)]
    public bool ValidateRegEx { get; set; }
}

public class UpdatePassCommandHandler : IRequestHandler<UpdatePassCommand, bool>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public UpdatePassCommandHandler(ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<bool> Handle(UpdatePassCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string token = await _identityService.GenerateResetToken(_currentUserService.Username!);
            var result = await _identityService.ResetPassword(_currentUserService.Username!, request.NewPassword!, token);
            if (!result.Succeeded)
                throw new Common.Exceptions.ValidationException("Error");
        }
        catch (Exception ex)
        {
            throw new ConflictException(ex.Message);
        }

        return true;
    }
}