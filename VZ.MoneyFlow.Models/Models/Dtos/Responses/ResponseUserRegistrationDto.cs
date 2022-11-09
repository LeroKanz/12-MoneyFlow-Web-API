using System.Collections.Generic;

namespace VZ.MoneyFlow.Models.Models.Dtos.Responses
{
    public class ResponseUserRegistrationDto : ResponseAuthResultDto
    {
        public ResponseUserRegistrationDto(bool Success, string Errors) : base(Success, Errors)
        {
        }

        public ResponseUserRegistrationDto(bool Success, List<string> Errors = null, string Token = null, string RefreshToken = null) : base(Success, Errors, Token, RefreshToken)
        {
        }
    }
}
