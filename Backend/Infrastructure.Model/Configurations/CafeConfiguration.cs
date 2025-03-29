using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Model.Configurations;

public class CafeConfiguration : BaseConfiguration<Cafe>
{
	public override void Configure(EntityTypeBuilder<Cafe> builder)
	{
		base.Configure(builder);

		builder.HasKey(x => x.Id);
		builder.Property(x => x.Name).IsRequired();
		builder.Property(x => x.Description).IsRequired();
		builder.Property(x => x.Logo);
		builder.Property(x => x.Location).IsRequired();
	}
}
