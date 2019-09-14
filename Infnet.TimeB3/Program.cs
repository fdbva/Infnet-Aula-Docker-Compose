using Infnet.TimeB3.OrderTicketContracts;
using Infnet.TimeB3.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Infnet.TimeB3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var azureStorageQueueService = new AzureStorageQueueService();
            var random = new Random();
            var randomNumber = random.Next(9999);
            Console.WriteLine($"Start... {randomNumber}");

            while (true)
            {
                try
                {
                    var orderTicketString = await azureStorageQueueService.DequeueAsync("order-ticket-command-queue");
                    var orderTicket = JsonConvert.DeserializeObject<OrderTicketCommand>(orderTicketString);
                    Console.WriteLine($"Ticket com id:[{orderTicketString}] lido com sucesso! {randomNumber}");

                    Console.WriteLine(orderTicketString);

                    var ticketOrdered = new TicketOrderedEvent {Id = orderTicket.Id};
                    var ticketOrderedSerialized = JsonConvert.SerializeObject(ticketOrdered);
                    await azureStorageQueueService.EnqueueAsync("ticket-ordered-event-queue-group-b",
                        ticketOrderedSerialized);
                    Console.WriteLine($"Ticket ordered! {randomNumber}");
                }
                catch
                {

                }
                finally
                {
                    Console.WriteLine($"Wait... {randomNumber}");
                    await Task.Delay(200);
                }
            }
        }
    }
}
