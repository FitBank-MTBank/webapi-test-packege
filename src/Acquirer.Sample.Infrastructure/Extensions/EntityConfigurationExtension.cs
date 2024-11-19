using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acquirer.Sample.Infrastructure.Persistence.Configurations;

public static class EntityConfigurationExtension
{
    public static EntityTypeBuilder SetPropertyCommums(this EntityTypeBuilder builder)
    {
        builder.Property("Created")
            .HasColumnName("created")
            .HasColumnType("timestamp(6)")
            .IsRequired();

        builder.Property("Updated")
                .HasColumnName("updated")
                .HasColumnType("timestamp(6)")
                .HasDefaultValue(null);

        builder.Property("Excluded")
                .HasColumnName("excluded")
                .IsRequired();
        
        return builder;
    }
}