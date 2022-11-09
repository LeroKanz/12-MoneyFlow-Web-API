using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using VZ.MoneyFlow.Entities.Enums;
using VZ.MoneyFlow.Models.Attribute;

namespace VZ.MoneyFlow.Models.Models.Dtos.Requests
{
    public class RequestOperationDto
    {
        [Required]        
        public decimal Amount { get; set; }
        [ValidEnum]
        public OperationType OperationType { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int CurrencyId { get; set; }
    }
}
