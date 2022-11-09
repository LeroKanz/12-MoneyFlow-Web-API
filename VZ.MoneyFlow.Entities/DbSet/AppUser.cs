using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace VZ.MoneyFlow.Entities.DbSet
{
    public class AppUser : IdentityUser
    {
        public List<Account> UserAccounts { get; set; }
        public List<AppUserAccount> AffiliatesAccounts { get; set; }
    }
}
