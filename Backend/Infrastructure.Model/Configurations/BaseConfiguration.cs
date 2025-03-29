using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Model.Configurations;

public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T>
	where T : class
{
	public virtual void Configure(EntityTypeBuilder<T> builder)
	{
		builder.ToTable(typeof(T).Name);
	}
}
