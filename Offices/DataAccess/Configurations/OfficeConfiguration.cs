using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offices.Models.Entities;

namespace Offices.DataAccess.Configurations;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Code).HasMaxLength(100);
        builder.Property(x => x.AddressCity).HasMaxLength(200).IsRequired();
        builder.Property(x => x.AddressRegion).HasMaxLength(200);
        builder.Property(x => x.AddressStreet).HasMaxLength(500);

        builder.ComplexProperty(x => x.Coordinates, coords => 
        {
            coords.Property(c => c.Latitude).HasColumnName("latitude");
            coords.Property(c => c.Longitude).HasColumnName("longitude");
        });

        builder.HasIndex(x => new { x.AddressCity, x.AddressRegion })
            .HasDatabaseName("ix_offices_city_region");

        builder.Property(x => x.Type)
            .HasConversion<string>().HasMaxLength(50);

        builder.HasMany(x => x.Phones)
            .WithOne(x => x.Office)
            .HasForeignKey(x => x.OfficeId)
            .OnDelete(DeleteBehavior.Cascade); // при удалении офиса, телефоны тоже удалятся
    }
}