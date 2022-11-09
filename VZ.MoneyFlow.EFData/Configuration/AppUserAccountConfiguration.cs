using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.EFData.Configuration
{
    public class AppUserAccountConfiguration : IEntityTypeConfiguration<AppUserAccount>
    {
        public void Configure(EntityTypeBuilder<AppUserAccount> builder)
        {
            builder.HasKey(appUserAccount => new
            {
                appUserAccount.AffiliateUserId,
                appUserAccount.AffiliateAccountId
            });
            builder.HasOne(appUserAccount => appUserAccount.AffiliateUser)
                .WithMany(appUser => appUser.AffiliatesAccounts).HasForeignKey(appUserAccount => appUserAccount.AffiliateUserId)
                .IsRequired();
            builder.HasOne(appUserAccount => appUserAccount.AffiliateAccount)
                .WithMany(account => account.Affiliates).HasForeignKey(appUserAccount => appUserAccount.AffiliateAccountId);
        }
    }
}
