using System;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.Entities.DbSet
{
    public class Operation
    {
        public int Id { get; set; }
        public DateTime OperationTime { get; set; }
        public decimal Amount { get; set; }
        public OperationType OperationType { get; set; }

        //Relationships
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
