using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.EFData.Configuration
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.HasOne(ac => ac.AccountFrom)
                .WithMany().HasForeignKey(ac => ac.AccountFromId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ac => ac.AccountTo)
                .WithMany().HasForeignKey(ac => ac.AccountToId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
