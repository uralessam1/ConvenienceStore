using Microsoft.AspNetCore.Identity;
using ConvenienceStoreApi.Domain.Entities;

namespace ConvenienceStoreApi.Infrastructure.Identity;

public class ApplicationRole : IdentityRole
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}