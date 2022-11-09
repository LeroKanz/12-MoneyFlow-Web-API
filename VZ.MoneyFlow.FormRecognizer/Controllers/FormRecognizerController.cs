using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using VZ.MoneyFlow.FR.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.FR.API.Controllers
{
    [Route("api/cheque")]
    public class FormRecognizerController : ControllerBase
    {
        protected ResponseDto _responseDto;
        private IFormRecognizerService _formRecognizerService;

        public FormRecognizerController(IFormRecognizerService formRecognizerService)
        {
            _formRecognizerService = formRecognizerService;
            _responseDto = new ResponseDto();
        }

        [HttpPost]
        public async Task<IActionResult> Recognize(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var result = await _formRecognizerService.RecognizeFile(stream);

            return Ok(result);
        }
    }
}
