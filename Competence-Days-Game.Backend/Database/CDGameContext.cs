using Compentence_Days_Game.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bystronic.Database
{
    public class CDGameContext : DbContext, IDbContext
    {

        public CDGameContext(DbContextOptions<CDGameContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Prize> Prize { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasKey(x => x.Email);
        }

        public async Task<bool> CanConnectAsync()
        {
            return await Database.CanConnectAsync();
        }

        public async Task<int> ExecuteRawAsync(string sql)
        {
            return await Database.ExecuteSqlRawAsync(sql);
        }

    }
}
