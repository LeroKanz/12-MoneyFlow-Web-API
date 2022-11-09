using System;

namespace VZ.MoneyFlow.Models.Models.Dtos
{
    public class TransferDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime OperationTime { get; set; }

        public int AccountFromId { get; set; }
        public AccountOperationDto AccountFrom { get; set; }

        public int AccountToId { get; set; }
        public AccountOperationDto AccountTo { get; set; }

        public int CurrencyId { get; set; }
        public CurrencyDto Currency { get; set; }
    }
}
