using BlazorEncuesta.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorEncuesta.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Encuesta> Encuestas => Set<Encuesta>();
    public DbSet<Pregunta> Preguntas => Set<Pregunta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Encuesta>(entity =>
        {
            entity.HasKey(e => e.Clave);
            entity.Property(e => e.Nombre).HasMaxLength(200).IsRequired();
            entity.HasIndex(e => e.FechaEmision);
            entity.HasIndex(e => e.FechaVencimiento);
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasKey(p => p.Clave);
            entity.Property(p => p.Nombre).HasMaxLength(500).IsRequired();
            entity.HasOne(p => p.Encuesta)
                  .WithMany(e => e.Preguntas)
                  .HasForeignKey(p => p.EncuestaClave)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}