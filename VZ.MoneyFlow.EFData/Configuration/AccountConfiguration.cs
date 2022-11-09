using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Drawing;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.EFData.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasOne(account => account.User).WithMany(appUser => appUser.UserAccounts);
        }
    }
}
