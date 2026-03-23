using BiodataService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BiodataService.Infrastructure.Persistence;

public class BiodataDbContext(DbContextOptions<BiodataDbContext> options) : DbContext(options)
{
    public DbSet<Biodata> Biodata => Set<Biodata>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Biodata>();
        entity.ToTable("biodata");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).HasColumnName("id");
        entity.Property(x => x.Nama).HasColumnName("nama").HasMaxLength(150).IsRequired();
        entity.Property(x => x.TanggalLahir).HasColumnName("tanggal_lahir").IsRequired();
        entity.Property(x => x.Alamat).HasColumnName("alamat");
        entity.Property(x => x.NoHp).HasColumnName("no_hp").HasMaxLength(20);
        entity.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
    }
}
