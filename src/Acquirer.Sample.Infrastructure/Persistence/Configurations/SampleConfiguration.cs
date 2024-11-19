using Acquirer.Sample.Domain.Enums;
using Acquirer.Shared.Utils.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Acquirer.Sample.Infrastructure.Persistence.Configurations;

public class SampleConfiguration : IEntityTypeConfiguration<Domain.Entities.Sample>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Sample> builder)
    {
        builder.ToTable("tb_agent_modality");
                
        builder.HasQueryFilter(x => !x.Excluded);

        builder.HasKey(p => p.SampleId);

        builder.Property(p => p.SampleId)
            .HasColumnName("sample_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(300)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(p => p.SampleType)
            .HasColumnName("sample_type_id")
            .HasConversion(new EnumToStringConverter<SampleTypeEnum>())
            .HasMaxLength(6)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(p => p.Brand)
            .HasColumnName("brand_id")
            .HasConversion(new EnumToStringConverter<BrandEnum>())
            .HasMaxLength(6)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(p => p.DateAt)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(p => p.Actived)
            .HasColumnName("active")
            .IsRequired();

        builder.SetPropertyCommums();
    }
}