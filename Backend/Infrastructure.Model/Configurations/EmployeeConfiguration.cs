using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Model.Configurations;

public class EmployeeConfiguration : BaseConfiguration<Employee>
{
	public override void Configure(EntityTypeBuilder<Employee> builder)
	{
		base.Configure(builder);

		builder.Property(x => x.Id).HasMaxLength(9).IsRequired();
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name).IsRequired();
		builder.Property(x => x.Name).IsRequired();
		builder.Property(x => x.Phone).IsRequired();
		builder.Property(x => x.Gender).HasMaxLength(1).IsRequired();

		builder.HasOne(x => x.Cafe).WithMany(x => x.Employees).HasForeignKey(e => e.CafeId);

		builder.Property(x => x.StartDate);
	}
}
