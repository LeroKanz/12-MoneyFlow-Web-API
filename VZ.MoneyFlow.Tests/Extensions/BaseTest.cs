using AutoMapper;
using VZ.MoneyFlow.EFData.Data;

namespace VZ.MoneyFlow.Tests.Extensions
{
    public class BaseTest
    {        
        protected static readonly IMapper _mapper;

        static BaseTest()
        {
            _mapper = CreateMapper();
        }

        private static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(AppMapper));
            });
            var mapper = mapperConfiguration.CreateMapper();
            return mapper;
        }
    }
}
