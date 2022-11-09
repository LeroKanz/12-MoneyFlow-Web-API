using System.ComponentModel.DataAnnotations;

namespace VZ.MoneyFlow.Models.Models.Dtos.Requests
{
    public class RequestUpdateCategoryDto
    {
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Less than 2 and more than 20!")]
        public string Name { get; set; }
    }
}
