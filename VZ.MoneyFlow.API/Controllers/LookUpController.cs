using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpController : ControllerBase
    {
        private static readonly Lazy<object> _lookUp = new(() => new 
        {
            AccountType = ToLookup<AccountType>(),
            CurrencyTypes = ToLookup<CurrencyType>(),            
            OperationType = ToLookup<OperationType>()
        });

        [HttpGet]
        public IActionResult GetAll()
        {            
            return Ok(_lookUp.Value);
        }

        private static Dictionary<int, string> ToLookup<T>() where T : struct, Enum
        {
            return Enum.GetValues<T>().Cast<T>().ToDictionary(t => Convert.ToInt32(t), t => t.ToString());
        }
    }    
}
