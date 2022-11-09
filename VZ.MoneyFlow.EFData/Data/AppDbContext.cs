using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.EFData.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<CurrencyExchange> CurrencyExchanges { get; set; }
        public DbSet<AccountCurrency> AccountsCurrencies { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserAccount> AppUsersAccounts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());   

            base.OnModelCreating(modelBuilder);
        }
    }
}
  