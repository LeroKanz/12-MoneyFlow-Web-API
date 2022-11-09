using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.EFData.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasOne(category => category.User).WithMany().HasForeignKey(category => category.UserId);
        }
    }
}
