using System.Collections.Generic;


namespace VZ.MoneyFlow.Models.Models.Dtos
{
    public class CurrencyAccountTypesDto
    {
        public Dictionary<int, string> AccountTypes { get; set; }
        public Dictionary<int, string> CurrencyTypes { get; set; }
    }
}
