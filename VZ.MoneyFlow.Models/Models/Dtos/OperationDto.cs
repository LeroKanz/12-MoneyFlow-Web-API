using System;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.Models.Models.Dtos
{
    public class OperationDto
    {
        public int Id { get; set; }
        public DateTime OperationTime { get; set; }
        public decimal Amount { get; set; }
        public OperationType OperationType { get; set; }

        //Relationships
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; }

        public int AccountId { get; set; }
        public AccountOperationDto Account { get; set; }

        public int CurrencyId { get; set; }
        public CurrencyDto Currency { get; set; }
    }
}
