using Microsoft.AspNetCore.Identity;
using ConvenienceStoreApi.Application.Common.Models;
using ConvenienceStoreApi.Application.Role.Models;
using ConvenienceStoreApi.Application.Account.Commands;
using ConvenienceStoreApi.Application.Security.Models;

namespace ConvenienceStoreApi.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GenerateResetToken(string userId, bool validateDelete = false);

    Task DeleteUserByUserId(string userId);

    Task<UsuarioWebModel> GetUserWebModel(string username);

    Task DeleteUserLogic(string? userId);

    Task<string> ReactiveUserLogic(string userName);

    string GetUserNameAsync(string userId);

    Dictionary<string, string> GetUsersNameAliasAsync();

    string GetNameByUsername(string username);

    Task<ApplicationUserDto> CheckPasswordAsync(string userName, string password);

    Task<IList<string>> GetRolesByUserAsync(string userName);

    Task<bool> ValidateUserToken(string? username);

    Task<bool> UsernameExist(string? username);

    Task<bool> IsValidPassword(string password);

    Task<IdentityResult> ResetPassword(string username, string password, string token);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<PaginatedList<ApplicationUserDto>> GetUsersAsync(int pagezise, int pagenumber);

    Task<List<RoleDto>> GetRolesAsync();

    Task<bool> ChangeNameUser(string userId, string? name);

}