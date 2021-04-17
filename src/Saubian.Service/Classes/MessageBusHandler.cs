using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Saubian.Service.Classes
{
    public static class MessageBusHandler
    {
        public static async Task PeepThemGlyphs(MessageSender messagesQueue, Stream messageStream)
        {
            string requestBody = await new StreamReader(messageStream).ReadToEndAsync();

            byte[] bytes = Encoding.ASCII.GetBytes(requestBody);
            Message m1 = new Message(bytes);
            await messagesQueue.SendAsync(m1);
        }
    }
}
