using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VZ.MoneyFlow.Models.Models.Dtos.Responses
{
    public class ResponseDto
    {
        public RecognizeDto Result { get; set; }
        public string DisplayMessage { get; set; }
        public List<string> ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}
