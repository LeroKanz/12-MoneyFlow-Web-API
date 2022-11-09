using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using VZ.MoneyFlow.FR.Models.Dtos;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface IFormRecognizerService
    {        
        Task<RecognizeDto> RecognizeFile(IFormFile file);
    }
}
