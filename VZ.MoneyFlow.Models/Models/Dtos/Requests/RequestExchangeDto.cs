using System.ComponentModel.DataAnnotations;

namespace VZ.MoneyFlow.Models.Models.Dtos.Requests
{
    public class RequestExchangeDto
    {
        [Required]
        public decimal AmountFrom { get; set; }
        [Required]
        public decimal AmountTo { get; set; }
        [Required]
        public int CurrencyFromId { get; set; }
        [Required]
        public int CurrencyToId { get; set; }
        [Required]
        public int AccountFromId { get; set; }
        [Required]
        public int AccountToId { get; set; }
    }
}
