using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;

namespace WorkerRoleWithSBQueue1
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue
        const string QueueName = "rowwo";

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");
            var eventDrivenMessagingOptions = new OnMessageOptions();
            eventDrivenMessagingOptions.AutoComplete = true;
            eventDrivenMessagingOptions.ExceptionReceived += OnExceptionReceived;
            eventDrivenMessagingOptions.MaxConcurrentCalls = 1;

            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            Client.OnMessage((receivedMessage) =>
                {
                    Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
                    Lebara.IN.BossC4BContracts.WorkOrder request = GetBody<Lebara.IN.BossC4BContracts.WorkOrder>(receivedMessage);
                    Trace.WriteLine("Processed Service Bus message: " + request.WorkOrderRequest.SubscriberNo);
                    Trace.WriteLine("Processed Service Bus message: " + receivedMessage.SequenceNumber.ToString());

                    Utility.ApplyPromotion(request.WorkOrderRequest.SubscriberNo.Substring(2), Convert.ToDouble(Regex.Match(request.WorkOrderRequest.ParaList[0].ParaValue, @"\d+").Value), request.WorkOrderRequest.SerialNo);
                    //try
                    //{
                    //    // Process the message


                    //}
                    //catch
                    //{
                    //    Trace.WriteLine("Processing Service Bus message:Exception Occurred");
                    //}
                }, eventDrivenMessagingOptions);

            CompletedEvent.WaitOne();
        }

       

        public T GetBody<T>(BrokeredMessage brokeredMessage)
        {
            var ct = brokeredMessage.ContentType;
            var stream = brokeredMessage.GetBody<Stream>();
            StreamReader reader = new StreamReader(stream);
            XmlSerializer serializer = new XmlSerializer(typeof(Lebara.IN.BossC4BContracts.WorkOrder));
            T msgBase = (T)serializer.Deserialize(reader);
            return msgBase;
        }

        static void OnExceptionReceived(object sender, ExceptionReceivedEventArgs e)
        {
            if (e != null && e.Exception != null)
            {
                Trace.WriteLine("Processing Service Bus message: " + e.Exception.Message);
                Console.WriteLine(" > Exception received: {0}", e.Exception.Message);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Initialize the connection to Service Bus Queue
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }
    }
}
