using System;

namespace VZ.MoneyFlow.Entities.DbSet
{
    public class Exchange
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }

        public int CurrencyFromId { get; set; }
        public int CurrencyToId { get; set; }

        public int AccountFromId { get; set; }
        public Account AccountFrom { get; set; }
        public int AccountToId { get; set; }
        public Account AccountTo { get; set; }
    }
}
