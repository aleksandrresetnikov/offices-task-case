using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offices.Models.Entities;

namespace Offices.DataAccess.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phones");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(x => x.Additional)
            .HasMaxLength(256);
        
        builder.HasIndex(x => x.OfficeId)
            .HasDatabaseName("IX_Phones_OfficeId");
    }
}