using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.EFData.Data
{
    public class AppDbInitializer
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();

                if (!context.Currencies.Any())
                {
                    context.Currencies.AddRange(new List<Currency>()
                    {
                        new Currency()
                        {
                             CurrencyType = CurrencyType.BYN
                        },
                        new Currency()
                        {
                             CurrencyType = CurrencyType.RUR
                        },
                        new Currency()
                        {
                             CurrencyType = CurrencyType.USD
                        },
                        new Currency()
                        {
                             CurrencyType = CurrencyType.EUR
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(new List<IdentityRole>()
                    {
                        new IdentityRole()
                        {
                             Name = "Admin",
                             NormalizedName = "ADMIN"
                        },
                        new IdentityRole()
                        {
                             Name = "User",
                             NormalizedName = "USER"
                        }                        
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
