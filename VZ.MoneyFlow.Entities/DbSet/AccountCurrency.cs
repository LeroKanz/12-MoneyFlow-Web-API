

namespace VZ.MoneyFlow.Entities.DbSet
{
    public class AccountCurrency
    {
        public decimal Amount { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        
    }
}
