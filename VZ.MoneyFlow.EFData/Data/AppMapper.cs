using AutoMapper;
using System.Collections.Generic;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Models.Models.Dtos;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.EFData.Data
{
    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<RequestAccountDto, Account>().ReverseMap();
            CreateMap<RequestUpdateAccountDto, Account>().ReverseMap();
            CreateMap<ResponseAccountDto, Account>().ReverseMap();
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<RequestAccountDto, AccountDto>()
                .ForMember(_ => _.AccountsCurrencies, x => x.MapFrom(_ => new List<AccountCurrencyDto> { new AccountCurrencyDto(_.Amount, default, _.CurrencyId) }))
                .ReverseMap();
            CreateMap<RequestAccountDto, ResponseAccountDto>().ReverseMap();
            CreateMap<Account, AccountOperationDto>().ReverseMap();
            CreateMap<AccountDto, ResponseAccountDto>().ReverseMap();
            CreateMap<AccountDto, RequestUpdateAccountDto>().ReverseMap();
            CreateMap<RequestUpdateAccountDto, ResponseAccountDto>().ReverseMap();
            CreateMap<RequestCreateCategoryDto, Category>().ReverseMap();
            CreateMap<RequestUpdateCategoryDto, Category>().ReverseMap();
            CreateMap<AccountCurrency, AccountCurrencyDto>().ReverseMap();
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Operation, OperationDto>().ReverseMap();
            CreateMap<RequestOperationDto, OperationDto>().ReverseMap();
            CreateMap<TransferDto, Transfer>().ReverseMap();
            CreateMap<RequestTransferDto, TransferDto>().ReverseMap();
            CreateMap<ExchangeDto, Exchange>().ReverseMap();
            CreateMap<RequestExchangeDto, ExchangeDto>().ReverseMap();
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<AppUserAccount, AppUserAccountAffiliateDto>().ReverseMap();
            CreateMap<AppUserAccount, AppUserAccountDto>().ReverseMap();
            CreateMap<AppUserAccountDto, AppUserAccountAffiliateDto>().ReverseMap();
        }
    }
}
