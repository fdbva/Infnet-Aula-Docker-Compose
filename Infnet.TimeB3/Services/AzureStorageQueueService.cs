using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

namespace Infnet.TimeB3.Services
{
    public class AzureStorageQueueService
    {
        private static CloudStorageAccount CloudStorageAccount { get; set; }

        public AzureStorageQueueService()
        {
            CloudStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=rocknet;AccountKey=Si8XtPsFKzkRNDif0YCtNLIFmy1WNNx0hRl22LRcLbW8wx3vHkuaKLlOTxLNeU1M8CNJKTQrxx0wsaF+SsCyjg==;EndpointSuffix=core.windows.net");
        }

        public async Task EnqueueAsync(string queueName, string message)
        {
            //Cria um objeto cliente de queue/fila a partir da conta de armazenamento
            var queueClient = CloudStorageAccount.CreateCloudQueueClient();

            //Crio um objeto que referencia uma determinada queue/fila
            var cloudQueue = queueClient.GetQueueReference(queueName);

            //Crio a fila se ela não existir
            await cloudQueue.CreateIfNotExistsAsync();

            //Crio uma mensagem a partir do objeto serializado
            var cloudQueueMessage = new CloudQueueMessage(message);

            //Adiciono a mensagem na queue/fila
            await cloudQueue.AddMessageAsync(cloudQueueMessage);
        }

        public async Task<string> DequeueAsync(string queueName)
        {
            //Cria um objeto cliente de queue/fila a partir da conta de armazenamento
            var queueClient = CloudStorageAccount.CreateCloudQueueClient();

            //Crio um objeto que referencia uma determinada queue/fila
            var cloudQueue = queueClient.GetQueueReference(queueName);

            //Crio a fila se ela não existir
            await cloudQueue.CreateIfNotExistsAsync();

            //Adiciono a mensagem na queue/fila
            var message = await cloudQueue.GetMessageAsync();
            await cloudQueue.DeleteMessageAsync(message);
            return message.AsString;
        }
    }
}
