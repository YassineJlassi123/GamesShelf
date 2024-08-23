using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Your_Connection_String");
             
        }
    }

    public DbSet<Game> Games { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasKey(g => g.Id);

        modelBuilder.Entity<Game>()
            .Property(g => g.Id)
            .ValueGeneratedOnAdd(); // Ensure Id is auto-generated
        modelBuilder.Entity<Game>()
     .Property(g => g.platforms)
     .HasColumnType("jsonb").HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v)
            ); ; // "jsonb" is more efficient than "json" in PostgreSQL


        base.OnModelCreating(modelBuilder);
    }
}