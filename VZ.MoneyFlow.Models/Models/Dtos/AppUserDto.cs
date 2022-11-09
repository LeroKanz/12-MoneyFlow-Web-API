using System.Collections.Generic;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.Models.Models.Dtos
{
    public class AppUserDto
    {
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
    }
}
