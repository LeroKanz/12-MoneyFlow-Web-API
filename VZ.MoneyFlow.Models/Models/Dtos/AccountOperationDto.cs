using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.Models.Models.Dtos
{
    public class AccountOperationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AccountType AccountType { get; set; }
        public string BankAccountNumber { get; set; }
        public int? LastFourDigitsOfCard { get; set; }
        public string UserId { get; set; }
    }
}
