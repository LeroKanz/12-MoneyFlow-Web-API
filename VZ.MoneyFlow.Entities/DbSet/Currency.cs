using System.Collections.Generic;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.Entities.DbSet
{
    public class Currency
    {
        public int Id { get; set; }
        public CurrencyType CurrencyType { get; set; }

        //Relationships
        public List<AccountCurrency> AccountsCurrencies { get; set; }
    }
}
