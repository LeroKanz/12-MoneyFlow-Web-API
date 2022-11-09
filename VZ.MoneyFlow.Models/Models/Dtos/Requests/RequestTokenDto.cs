using System.ComponentModel.DataAnnotations;

namespace VZ.MoneyFlow.Models.Models.Dtos.Requests
{
    public class RequestTokenDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
