using System.Diagnostics;
using System.Net;
using System.Threading;
using FrontEndWebRole.Models;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace OrderProcessingRole
{
    public class WorkerRole : RoleEntryPoint
    {
        // Le nom de votre file d'attente
        const string QueueName = "OrdersQueue";

        // QueueClient est thread-safe. Il est recommandé de le mettre en cache 
        // plutôt que de le recréer à chaque requête
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.WriteLine("Début du traitement des messages");

            // Démarre la pompe de messages et le rappel est appelé pour chaque message reçu, l'appel de Close sur le client arrêtant la pompe.
            Client.OnMessage((receivedMessage) =>
                {
                    try
                    {
                        // Traitement du message
                        Trace.WriteLine("Traitement du message Service Bus : " + receivedMessage.SequenceNumber.ToString());
                        Trace.WriteLine("Processing", receivedMessage.SequenceNumber.ToString());
                        // View the message as an OnlineOrder.
                        OnlineOrder order = receivedMessage.GetBody<OnlineOrder>();
                        Trace.WriteLine(order.Customer + ": " + order.Product, "ProcessingMessage");
                        receivedMessage.Complete();
                    }
                    catch
                    {
                        // Gérer ici toutes exceptions spécifiques du traitement des messages
                    }
                });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Définissez le nombre maximal de connexions simultanées 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Créez la file d'attente si elle n'existe pas déjà
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Initialisez la connexion à la file d'attente Service Bus
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Fermez la connexion à la file d'attente Service Bus
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }
    }
}