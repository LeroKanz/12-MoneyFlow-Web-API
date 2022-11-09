using System.ComponentModel.DataAnnotations;
using VZ.MoneyFlow.Entities.Enums;
using VZ.MoneyFlow.Models.Attribute;

namespace VZ.MoneyFlow.Models.Models.Dtos.Requests
{
    public class RequestUpdateAccountDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Less than 2 and more than 20!")]
        public string Name { get; set; }
        [ValidEnum]
        public AccountType AccountType { get; set; }
        public string BankAccountNumber { get; set; }
        public int? LastFourDigitsOfCard { get; set; }
    }
}
