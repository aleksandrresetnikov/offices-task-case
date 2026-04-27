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

        builder.ComplexProperty(x => x.Coordinates);

        builder.HasIndex(x => new { x.AddressCity, x.AddressRegion });

        builder.Property(x => x.Type)
            .HasConversion<string>();

        builder.HasMany(x => x.Phones)
            .WithOne(x => x.Office)
            .HasForeignKey(x => x.OfficeId)
            .OnDelete(DeleteBehavior.Cascade); // при удалении офиса, телефоны тоже удалятся
    }
}