using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ConvenienceStoreApi.Domain.Entities;

namespace ConvenienceStoreApi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }

    Task<int> ExecuteSqlCommandAsync(string sql, SqlParameter[]? array);

    Task<List<T>> ExecuteSelectQueryAsync<T>(string sql, SqlParameter[] parameters);

    //Task<int> ExecuteSqlCommand2(FormattableString sql);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<Tbl_Product> Tbl_Product { get; }

}