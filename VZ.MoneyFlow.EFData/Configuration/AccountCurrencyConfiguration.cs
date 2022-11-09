using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.EFData.Configuration
{
    public class AccountCurrencyConfiguration : IEntityTypeConfiguration<AccountCurrency>
    {
        public void Configure(EntityTypeBuilder<AccountCurrency> builder)
        {
            builder.HasKey(accountCurrency => new
            {
                accountCurrency.AccountId,
                accountCurrency.CurrencyId
            });
            builder.HasOne(accountCurrency => accountCurrency.Account)
                .WithMany(account => account.AccountsCurrencies).HasForeignKey(accountCurrency => accountCurrency.AccountId)
                .IsRequired();
            builder.HasOne(accountCurrency => accountCurrency.Currency)
                .WithMany(currency => currency.AccountsCurrencies).HasForeignKey(accountCurrency => accountCurrency.CurrencyId);
        }
    }
}
