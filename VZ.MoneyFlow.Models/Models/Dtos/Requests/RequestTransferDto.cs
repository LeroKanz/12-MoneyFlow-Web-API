using System.ComponentModel.DataAnnotations;

namespace VZ.MoneyFlow.Models.Models.Dtos.Requests
{
    public class RequestTransferDto
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int AccountFromId { get; set; }
        [Required]
        public int AccountToId { get; set; }
        [Required]
        public int CurrencyId { get; set; }
    }
}
