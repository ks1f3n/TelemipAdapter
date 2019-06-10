using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TelemipAdapter.Models.Gaps;
using TelemipAdapter.Models.Incls;

namespace TelemipAdapter
{
    public class SensorDbContext : DbContext
    {
        public DbSet<Gap> Gap { get; set; }
        public DbSet<Incl> Incl { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "sensor.db", Cache = SqliteCacheMode.Shared };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
    }
}
