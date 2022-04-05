using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bystronic.Database
{
    public interface IDbContext : IDisposable
    {
		EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		DbSet<TEntity> Set<TEntity>() where TEntity : class;

		Task<bool> CanConnectAsync();
		Task<int> ExecuteRawAsync(string sql);

	}
}
