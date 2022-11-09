using System.Collections.Generic;

namespace VZ.MoneyFlow.Models.Models.Dtos.Responses
{
    public class ResponseAuthResultDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }

        public ResponseAuthResultDto(bool Success, string Errors)
        {
            this.Success = Success;
            this.Errors = new List<string> { Errors };
        }

        public ResponseAuthResultDto(bool Success, List<string> Errors = null, string Token = null, string RefreshToken = null)
        {
            this.Success = Success;
            this.Errors = Errors;
            this.Token = Token;
            this.RefreshToken = RefreshToken;
        }
    }
}
