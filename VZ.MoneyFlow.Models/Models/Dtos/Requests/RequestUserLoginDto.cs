using System.ComponentModel.DataAnnotations;

namespace VZ.MoneyFlow.Models.Models.Dtos.Requests
{
    public class RequestUserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
