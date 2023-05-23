using Microsoft.EntityFrameworkCore;
using SaritasaT.Models;
using System.Reflection.Metadata;

namespace SaritasaT.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base (options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Storage>()
                .HasMany(e => e.Items)
                .WithOne(e => e.Storage)
                .HasForeignKey(e => e.StorageId)
                .IsRequired();
            modelBuilder.Entity<StorageItem>().HasIndex(x => new { x.FileName }).IsUnique();
        }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<User> Users  { get; set; }
        public DbSet<StorageItem> StorageItems { get; set; }
    }
}
