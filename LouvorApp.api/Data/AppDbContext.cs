using LouvorApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LouvorApp.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Setlist> Setlists { get; set; }
        public DbSet<SetlistSong> SetlistSongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurando a Chave Primária Composta para a tabela de junção N:N
            modelBuilder.Entity<SetlistSong>()
                .HasKey(ss => new { ss.SetlistId, ss.SongId });

            // Relacionamento Setlist -> SetlistSong
            modelBuilder.Entity<SetlistSong>()
                .HasOne(ss => ss.Setlist)
                .WithMany(s => s.SetlistSongs)
                .HasForeignKey(ss => ss.SetlistId);

            // Relacionamento Song -> SetlistSong
            modelBuilder.Entity<SetlistSong>()
                .HasOne(ss => ss.Song)
                .WithMany(s => s.SetlistSongs)
                .HasForeignKey(ss => ss.SongId);
        }
    }
}