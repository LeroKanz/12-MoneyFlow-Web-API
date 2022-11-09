using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.FR.API.Messages
{
    public class AzureServiceBusConcumer
    {
        private async Task OnMessageReceived(ProcessMessageEventArgs processMessageEventArgs)
        {
            var message = processMessageEventArgs.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            ResponseDto responseDto = JsonConvert.DeserializeObject<ResponseDto>(body);
        }
    }
}
