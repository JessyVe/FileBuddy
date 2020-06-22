using DatabaseConnection.Database.Mappings;
using Microsoft.EntityFrameworkCore;
using SharedRessources.Database.Mappings;
using SharedRessources.Dtos;

namespace SharedRessources.Database
{
    public class SQLiteDBContext : DbContext
    {
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<SharedFile> SharedFile { get; set; }
        public DbSet<DownloadTransaction> DownloadTransaction { get; set; }

        public DbSet<AuthorizedAccess> AuthorizedAccess { get; set; }

        private const string DatabaseSource = "Resources/file-buddy.db";

        public SQLiteDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DatabaseSource}");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new AppUserMap(builder.Entity<AppUser>());
            new SharedFileMap(builder.Entity<SharedFile>());
            new DownloadTransactionMap(builder.Entity<DownloadTransaction>());
            new AuthorizedAccessMap(builder.Entity<AuthorizedAccess>());
        }
    }
}
