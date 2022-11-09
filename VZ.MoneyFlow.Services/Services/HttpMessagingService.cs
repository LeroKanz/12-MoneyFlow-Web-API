using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.Enums;
using VZ.MoneyFlow.FR.Models.Dtos;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.Services.Services
{
    public class HttpMessagingService : IHttpMessagingService
    {
        public ResponseDto ResponseDto { get; set; }
        public IHttpClientFactory httpClientFactory { get; set; }

        public HttpMessagingService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            ResponseDto = new ResponseDto();
        }

        public async Task<RecognizeDto> SendAsync(APIRequest apiRequest)
        {
            try
            {
                var client = httpClientFactory.CreateClient("FormRecognizer");
                var message = new HttpRequestMessage();
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                if (apiRequest.Data != null)
                {
                    var formContent = new MultipartFormDataContent();
                    formContent.Add(new StreamContent(apiRequest.Data.OpenReadStream()), "file", 
                        apiRequest.Data.FileName);
                    message.Content = formContent;
                }

                HttpResponseMessage apiResponse = null;
                switch (apiRequest.APIType)
                {
                    case APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<RecognizeDto>(apiContent);
                return response;
            }
            catch (Exception ex)
            {
                var newResponseDto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessage = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false,
                };

                var result = JsonConvert.SerializeObject(newResponseDto);
                var response = JsonConvert.DeserializeObject<RecognizeDto>(result);
                return response;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
