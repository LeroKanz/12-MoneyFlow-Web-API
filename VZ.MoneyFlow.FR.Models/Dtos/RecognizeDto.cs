using System;
using System.Collections.Generic;

namespace VZ.MoneyFlow.FR.Models.Dtos
{
    public class RecognizeDto
    {
        public DateTimeOffset TransactionDate { get; set; }
        public double Total { get; set; }
        public string MerchantName { get; set; }
        public List<string> ItemDescriptions { get; set; }
        public List<double> ItemPrices { get; set; }
    }
}
