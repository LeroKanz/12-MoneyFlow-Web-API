using Grpc.Core;
using System.IO;
using System;
using System.Threading.Tasks;
using VZ.MoneyFlow.FR.IData.IServices;
using NuGet.Protocol.Plugins;
using System.Net.Http;
using VZ.MoneyFlow.Models.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace VZ.MoneyFlow.FR.API.Grpc
{
    public class GrpcFormRecognizerService : GrpcFormRecognizer.GrpcFormRecognizerBase
    {
        private readonly IFormRecognizerService _formRecognizerService;
        public GrpcFormRecognizerService(IFormRecognizerService formRecognizerService)
        {
            _formRecognizerService = formRecognizerService;
        }

        public override async Task<FormRecognizeResponse> RecognizeFile(FormRecognizeRequest formRecognizeRequest,
            ServerCallContext serverCallContext)
        {
            MemoryStream mS;
            using (var memorySrteam = new MemoryStream())
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(formRecognizeRequest.File);
                memorySrteam.Write(byteArray, 0, byteArray.Length);
                mS = memorySrteam;
            }

            var recognizedResult = await _formRecognizerService.RecognizeFile(mS);
            var result = new FormRecognizeResponse()
            {
                MerchantName = recognizedResult.MerchantName,
                Total = recognizedResult.Total
            };

            return result;
        }
    }
}
