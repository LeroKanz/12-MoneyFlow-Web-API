using System.Collections.Generic;

namespace VZ.MoneyFlow.Models.Models.Dtos.Responses
{
    public class ResponseUserLoginDto : ResponseAuthResultDto
    {
        public ResponseUserLoginDto(bool Success, string Errors) : base(Success, Errors)
        {
        }

        public ResponseUserLoginDto(bool Success, List<string> Errors = null, string Token = null, string RefreshToken = null) : base(Success, Errors, Token, RefreshToken)
        {
        }
    }
}
