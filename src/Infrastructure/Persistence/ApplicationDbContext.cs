//using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ConvenienceStoreApi.Application.Common.Interfaces;
using ConvenienceStoreApi.Infrastructure.Common;
using ConvenienceStoreApi.Infrastructure.Identity;
using ConvenienceStoreApi.Infrastructure.Persistence.Interceptors;
using ConvenienceStoreApi.Domain.Entities;

namespace ConvenienceStoreApi.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    public DbSet<Tbl_Product> Tbl_Product => Set<Tbl_Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }

        builder.Entity<ApplicationUser>(b =>
        {
            b.Property(p => p.Id)
            .HasColumnName("UserId");
            b.HasMany(e => e.UserRoles).WithOne(e => e.User).HasForeignKey(ur => ur.UserId).IsRequired();
            b.HasIndex(e => e.Nombre).IsUnique();
            b.HasIndex(e => e.Email).IsUnique();
            b.Property(p => p.IsActive)
             .HasDefaultValue(true);
            b.Property(p => p.IsDeleted)
             .HasDefaultValue(false);
        });

        builder.Entity<ApplicationRole>(b =>
        {
            b.HasMany(e => e.UserRoles).WithOne(e => e.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public async Task<int> ExecuteSqlCommandAsync(string sql, SqlParameter[]? array)
    {
        return await base.Database.ExecuteSqlRawAsync(sql, array);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<T>> ExecuteSelectQueryAsync<T>(string sql, SqlParameter[] parameters)
    {
        var result = new List<T>();

        using (var command = base.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = sql;
            command.Parameters.AddRange(parameters);
            base.Database.OpenConnection();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var item = Activator.CreateInstance<T>();
                    var properties = typeof(T).GetProperties();

                    foreach (var property in properties)
                    {
                        var ordinal = reader.GetOrdinal(property.Name);
                        if (!reader.IsDBNull(ordinal))
                        {
                            var value = reader.GetValue(ordinal);
                            property.SetValue(item, value);
                        }
                    }

                    result.Add(item);
                }
            }
        }

        return result;
    }
}