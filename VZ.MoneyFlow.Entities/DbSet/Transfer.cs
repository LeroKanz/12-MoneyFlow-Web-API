using System;


namespace VZ.MoneyFlow.Entities.DbSet
{
    public class Transfer
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime OperationTime { get; set; }

        //Relationships
        public int AccountFromId { get; set; }
        public Account AccountFrom { get; set; }
        public int AccountToId { get; set; }
        public Account AccountTo { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}