using Microsoft.AspNetCore.Identity;
using ConvenienceStoreApi.Domain.Entities;

namespace ConvenienceStoreApi.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string Nombre { get; set; }
    public DateTime CreateDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public string? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}