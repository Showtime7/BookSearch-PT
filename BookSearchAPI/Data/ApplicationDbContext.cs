using Microsoft.EntityFrameworkCore;
using BookSearchAPI.Models;

namespace BookSearchAPI.Data
{
    // Contexto principal de la base de datos
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de User (Índice único)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Configuración de Favorite (Índice compuesto único)
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.ExternalId })
                .IsUnique(); // Evita duplicados por usuario

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
