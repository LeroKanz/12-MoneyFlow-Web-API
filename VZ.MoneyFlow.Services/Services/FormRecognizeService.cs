using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.Enums;
using VZ.MoneyFlow.FR.Models.Dtos;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models;
using VZ.MoneyFlow.Models.Models.Configuration;

namespace VZ.MoneyFlow.Services.Services
{
    public class FormRecognizeService : HttpMessagingService, IFormRecognizerService
    {
        private readonly FormRecognizer _formRecognizer;
        private readonly IHttpClientFactory _httpClientFactory;

        public FormRecognizeService(IOptionsMonitor<FormRecognizer> options, 
            IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _formRecognizer = options.CurrentValue;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RecognizeDto> RecognizeFile(IFormFile file)
        {
            var result = await SendAsync(new APIRequest()
            {
                APIType = APIType.POST,
                Data = file,
                Url = FRBase.FRAPIBase + "/api/cheque",
                Token = ""
            });

            return result;
        }
    }
}
