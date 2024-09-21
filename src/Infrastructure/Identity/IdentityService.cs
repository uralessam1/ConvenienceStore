using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Application.Common.Exceptions;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Application.Common.Models;
using ConvenienceStoreApi.Application.Role.Models;
using ConvenienceStoreApi.Application.Security.Models;


namespace ConvenienceStoreApi.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<UsuarioWebModel> GetUserWebModel(string username)
    {
        ApplicationUser user = _userManager.Users.FirstOrDefault(u => u.NormalizedUserName == username.ToUpper()) ?? throw new ValidationException("Usuario no encontrado");
        return new UsuarioWebModel
        {
            CreateDate = user.CreateDate,
            UserId = user.Id,
            Nombre = user.Nombre,
            Email = user.UserName,
            Role = (await _userManager.GetRolesAsync(user)).First(),
        };
    }

    public string GetUserNameAsync(string userId)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        return user == null ? throw new ValidationException("Usuario no encontrado") : user.UserName;
    }

    public Dictionary<string, string> GetUsersNameAliasAsync()
    {
        var userDictionary = _userManager.Users
        .ToDictionary(user => user.Id, user => user.Nombre);

        return userDictionary;
    }

    public string GetNameByUsername(string username)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.NormalizedUserName == username.ToUpper());
        if (user == null)
            throw new ValidationException("Usuario no encontrado");
        return user.Nombre;
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        int count = _roleManager.Roles.Count();
        if (count < 1)
            throw new ValidationException("No hay roles");

        var roles = _roleManager.Roles.OrderBy(x => x.Name)
                       .Select(r => new RoleDto
                       {
                           id = r.Id,
                           Name = r.Name,
                       })
                       .ToList();
        return roles;
    }

    public async Task<PaginatedList<ApplicationUserDto>> GetUsersAsync(int pagezise, int pagenumber)
    {
        int count = _userManager.Users.Where(x => x.UserName != "master@convenienceStore.com.mx" && x.UserName != "robot_convenienceStore" && x.UserName != "Master@MFS.com" && x.IsActive == true).Count();
        if (count < 1)
            throw new ValidationException("No hay usuarios");

        var users = _userManager.Users.Include(u => u.UserRoles).Where(x => x.UserName != "master@convenienceStore.com.mx" && x.UserName != "robot_convenienceStore" && x.UserName != "Master@MFS.com" && x.IsActive == true)
                       .Select(u => new ApplicationUserDto
                       {
                           UserId = u.Id,
                           Role = u.UserRoles.First().Role.Name,
                           Email = u.Email,
                           Username = u.UserName,
                           Name = u.Nombre,
                           IsActive = u.IsActive
                       })
                       .Skip(pagezise * pagenumber)
                       .Take(pagezise)
                       .ToList();

        PaginatedList<ApplicationUserDto> res = new(users, count, pagenumber, pagezise);
        return res;
    }

    public async Task<ApplicationUserDto> CheckPasswordAsync(string userName, string password)
    {
        var userApp = await _userManager.FindByNameAsync(userName) ?? throw new ValidationException("Usuario no encontrado");
        return !userApp.IsActive
            ? throw new ValidationException("El usuario esta bloqueado")
            : !(await _userManager.CheckPasswordAsync(userApp, password))
            ? throw new ConflictException("Usuario o contraseña incorrectos")
            : new ApplicationUserDto
            {
                Email = userApp.Email,
                Name = userApp.Nombre,
                UserId = userApp.Id,
                Username = userApp.UserName
            };
    }

    public async Task<IList<string>> GetRolesByUserAsync(string userName)
    {
        var userApp = await _userManager.FindByNameAsync(userName) ?? throw new ValidationException("Usuario no encontrado");
        var roles = await _userManager.GetRolesAsync(userApp);

        return roles;
    }

    public async Task DeleteUserLogic(string? userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ValidationException("Usuario no encontrado");
        if (!user.IsActive)
            throw new ValidationException("Usuario previamente borrado");
        user.IsActive = false;
        await _userManager.UpdateAsync(user);
    }

    public async Task<bool> ValidateUserToken(string? userId)
    {
        var user = (await _userManager.FindByIdAsync(userId));
        return user != null && user.IsActive;
    }

    public async Task<bool> ChangeNameUser(string userId, string? name)
    {
        var user = (await _userManager.FindByIdAsync(userId));
        user.Nombre = name ?? user.Nombre;
        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<bool> UsernameExist(string? username)
    {
        var x = (await _userManager.FindByEmailAsync(username));
        return x?.IsActive ?? false;
    }

    public async Task<bool> IsValidPassword(string password)
    {
        var validators = _userManager.PasswordValidators;
        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(_userManager, null, password);

            if (!result.Succeeded)
            {
                return false;
            }
        }
        return true;
    }

    public async Task DeleteUserByUserId(string userId)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
            await _userManager.DeleteAsync(user);
    }

    public async Task<string> GenerateResetToken(string username, bool validateDelete = false)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(username) ?? throw new ValidationException("Usuario no encontrado");
            if (validateDelete && user.IsActive)
                throw new ValidationException("Usuario no encontrado");
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IdentityResult> ResetPassword(string username, string password, string token)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username) ?? throw new ValidationException("Usuario no encontrado");
            IdentityResult res = await _userManager.ResetPasswordAsync(user, token, password);
            return res;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ValidationException("Usuario no encontrado");
        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<string> ReactiveUserLogic(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
            return "";
        if (user.IsActive)
            throw new ValidationException("Nombre de usuario existente");
        user.IsActive = true;
        await _userManager.UpdateAsync(user);

        return user.Id;
    }
}