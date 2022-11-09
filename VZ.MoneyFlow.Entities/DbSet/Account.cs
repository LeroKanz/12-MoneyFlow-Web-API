using System.Collections.Generic;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.Entities.DbSet
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AccountType AccountType { get; set; }
        public string BankAccountNumber { get; set; }
        public int? LastFourDigitsOfCard { get; set; }

        //Relationships
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public List<AppUserAccount> Affiliates { get; set; }
        public List<AccountCurrency> AccountsCurrencies { get; set; }
    }
}
