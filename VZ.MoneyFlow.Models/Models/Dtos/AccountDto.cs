﻿using System.Collections.Generic;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.Models.Models.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AccountType AccountType { get; set; }
        public string BankAccountNumber { get; set; }
        public int? LastFourDigitsOfCard { get; set; }
        public string UserId { get; set; }
        public List<AccountCurrencyDto> AccountsCurrencies { get; set; }
        public List<AppUserAccountDto> Affiliates { get; set; }
    }
}
