using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Account.Commands;

namespace ConvenienceStoreApi.Application.Security.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UpdateUserCommandValidator(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        
    }
}