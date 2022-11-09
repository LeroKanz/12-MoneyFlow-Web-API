using System;
using System.Threading.Tasks;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;
using VZ.MoneyFlow.Models.Models;
using VZ.MoneyFlow.FR.Models.Dtos;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface IHttpMessagingService : IDisposable
    {
        ResponseDto ResponseDto { get; set; }
        Task<RecognizeDto> SendAsync(APIRequest apiRequest);
    }
}
