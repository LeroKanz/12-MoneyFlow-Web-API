using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using VZ.MoneyFlow.FR.API;
using VZ.MoneyFlow.FR.Models.Dtos;
using VZ.MoneyFlow.IData.IServices;
using static VZ.MoneyFlow.FR.API.GrpcFormRecognizer;

namespace VZ.MoneyFlow.API.Grpc
{
    public class FormRecognizeGrpcService : IFormRecognizerService
    {
        private readonly GrpcFormRecognizerClient _grpcFormRecognizerClient;

        public FormRecognizeGrpcService(GrpcFormRecognizerClient grpcFormRecognizerClient)
        {
            _grpcFormRecognizerClient = grpcFormRecognizerClient;
        }

        public async Task<RecognizeDto> RecognizeFile(IFormFile file)
        {
            string bytesToString;
            using (var memorySrteam = new MemoryStream())
            {
                file.CopyTo(memorySrteam);
                var fileBytes = memorySrteam.ToArray();
                bytesToString = Convert.ToBase64String(fileBytes);
            }
            var response = await _grpcFormRecognizerClient.RecognizeFileAsync(new FormRecognizeRequest
            {
                File = bytesToString
            });

            return new RecognizeDto { MerchantName = response.MerchantName, Total = response.Total };
        }
    }
}
