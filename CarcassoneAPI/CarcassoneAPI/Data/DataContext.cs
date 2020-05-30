using CarcassoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarcassoneAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardComponent> BoardComponents { get; set; }

        public DbSet<Tile> Tiles { get; set; }
        public DbSet<TileComponent> TileComponents { get; set; }

        public DbSet<TileType> TileTypes { get; set; }
        public DbSet<TileTypeComponent> TileTypeComponents { get; set; }
        public DbSet<TileTypeTerrain> TileTypeTerrains { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Board>().Property(b => b.Width).IsRequired();
            builder.Entity<Board>().Property(b => b.Height).IsRequired();
            builder.Entity<Board>().Property(b => b.Name).HasMaxLength(128);

            builder.Entity<TileType>().Property(b => b.Name).HasMaxLength(128);
            builder.Entity<TileType>().Property(b => b.ImageUrl).HasMaxLength(128);

            builder.Entity<Tile>().HasIndex(t => new { t.BoardId, t.X, t.Y }).IsUnique();
            builder.Entity<Tile>().Property(t => t.X).IsRequired();
            builder.Entity<Tile>().Property(t => t.Y).IsRequired();

            builder.Entity<Tile>()
                .HasOne(t => t.Board)
                .WithMany(b => b.Tiles)
                .HasForeignKey(t =>t.BoardId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<Tile>()
                .HasOne(t => t.TileType)
                .WithMany()
                .HasForeignKey(t => t.TileTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Entity<TileTypeTerrain>().Property(t => t.TerrainType).IsRequired();
            builder.Entity<TileTypeTerrain>().Property(t => t.Position).IsRequired();

            //builder.Entity<TileTypeTerrain>()
            //    .HasKey(k => new { k.TileTypeId, k.Position });

            //builder.Entity<TileComponent>()
            //    .HasKey(k => new { k.TileId, k.TileTypeComponentId });

            builder.Entity<TileComponent>()
                .HasOne(c => c.Tile)
                .WithMany(t => t.Components)
                .HasForeignKey(c => c.TileId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Entity<TileComponent>()
                .HasOne(c => c.TileTypeComponent)
                .WithMany()
                .HasForeignKey(c => c.TileTypeComponentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Entity<TileComponent>()
                .HasOne(c => c.BoardComponent)
                .WithMany(b => b.Components)
                .HasForeignKey(c => c.BoardComponentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        }
    }
}
