using System.IO;
using System.Threading.Tasks;
using VZ.MoneyFlow.FR.Models.Dtos;

namespace VZ.MoneyFlow.FR.IData.IServices
{
    public interface IFormRecognizerService
    {
        Task<RecognizeDto> RecognizeFile(Stream stream);
    }
}
