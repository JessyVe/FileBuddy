using DatabaseConnection.Database.Mappings;
using Microsoft.EntityFrameworkCore;
using SharedRessources.Dtos;

namespace SharedRessources.Database
{
    public class SQLiteDBContext : DbContext
    {
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<SharedFile> SharedFiles { get; set; }

        public DbSet<FileTransaction> FileTransactions { get; set; }
        public DbSet<Receiver> Receivers { get; set; }

        private const string DatabaseSource = "resources/file-buddy-02.db";

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DatabaseSource}");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new AppUserMap(builder.Entity<AppUser>());
            new SharedFileMap(builder.Entity<SharedFile>());
            new FileTransactionMap(builder.Entity<FileTransaction>());
            new ReceiverMap(builder.Entity<Receiver>());
        }
    }
}
