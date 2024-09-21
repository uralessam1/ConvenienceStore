using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Infrastructure.Common;
using ConvenienceStoreApi.Infrastructure.Identity;

namespace ConvenienceStoreApi.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMediator _mediator;
    private readonly IDateTime _dateTime;

    public ApplicationDbContextInitialiser(IMediator mediator, IDateTime dateTime, ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _mediator = mediator;
        _dateTime = dateTime;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        //Create Roles
        List<ApplicationRole> roles = new List<ApplicationRole>
        {
            new ApplicationRole { Name = Roles.Master},
            new ApplicationRole { Name = Roles.User},
            new ApplicationRole { Name = Roles.Robot},
        };
        if (!_roleManager.Roles.Any())
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }

        //CREATE USER
        if (!_userManager.Users.Any())
        {
            var master = new ApplicationUser { CreateDate = _dateTime.Now, UserName = "master@convenienceStore.com.mx", Email = "master@convenienceStore.com.mx", Nombre = "Master" };
            var res = await _userManager.CreateAsync(master, "9$g7T7MrP+Qc8kp£W;E*%j+t,*");

            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(master, Roles.Master);
                await _context.SaveChangesAsync();
            }
        }
        
       
    }
  
}