using System;

namespace VZ.MoneyFlow.Entities.DbSet
{
    public class CurrencyExchange
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime OperationTime { get; set; }
        public AccountCurrency AccountCurrencyFrom { get; set; }
        public AccountCurrency AccountCurrencyTo { get; set; }
    }
}
