using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.Enums;

namespace VZ.MoneyFlow.Models.Models
{
    public class APIRequest
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public IFormFile Data { get; set; }
        public string Token { get; set; }
    }
}
