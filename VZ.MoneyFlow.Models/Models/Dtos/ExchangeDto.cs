using System;
using VZ.MoneyFlow.Entities.DbSet;

namespace VZ.MoneyFlow.Models.Models.Dtos
{
    public class ExchangeDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }

        public int CurrencyFromId { get; set; }
        public int CurrencyToId { get; set; }

        public int AccountFromId { get; set; }
        public AccountOperationDto AccountFrom { get; set; }
        public int AccountToId { get; set; }
        public AccountOperationDto AccountTo { get; set; }
    }
}
