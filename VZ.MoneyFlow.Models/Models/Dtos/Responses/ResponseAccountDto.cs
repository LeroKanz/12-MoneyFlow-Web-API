using System.Collections.Generic;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.Models.Models.Dtos.Responses
{
    public class ResponseAccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AccountType AccountType { get; set; }
        public int? LastFourDigitsOfCard { get; set; }        

        public List<AccountCurrencyDto> AccountsCurrencies { get; set; }
        public List<AppUserAccountAffiliateDto> Affiliates { get; set; }
    }
}
