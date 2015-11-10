using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
using Microsoft.Practices.TransientFaultHandling;

namespace Lebara.TopUpService.RAF.Service
{
    /// <summary>
    /// This class processes the messages from service bus queue.
    /// </summary>
   public class ProcessMessageQueue
    {
       
       /// <summary>
       /// Retry policy object.
       /// </summary>
       private static RetryPolicy<ServiceBusTransientErrorDetectionStrategy> retryPolicy;

       /// <summary>
       /// queue name
       /// </summary>
       private static string queueName;

       /// <summary>
       /// Gets namespace base address.
       /// </summary>
       private static string baseAddress;

       /// <summary>
       /// Gets issuer name.
       /// </summary>
       private static string issuerName;

       /// <summary>
       /// Gets issuer key.
       /// </summary>
       private static string issuerKey;

       /// <summary>
       /// Service bus Uri.
       /// </summary>
       private static Uri serviceBusUri;

       /// <summary>
       /// Namespace manager.
       /// </summary>
       private static NamespaceManager namespaceManager;

       /// <summary>
       /// Messaging factory.
       /// </summary>
       private static MessagingFactory messagingFactory;


       private static string namespacemanager;

       #region Private properties


       /// <summary>
       /// Gets base queuename.
       /// </summary>
       private static string QueueName
       {
           get
           {
               if (string.IsNullOrEmpty(queueName))
               {
                   queueName = ConfigValues.QueueName;
               }

               return queueName;
           }
       }

       /// <summary>
       /// Gets base address.
       /// </summary>
       private static string BaseAddress
       {
           get
           {
               if (string.IsNullOrEmpty(baseAddress))
               {
                   baseAddress = ConfigValues.ServiceBusNamespaceAddress;
               }

               return baseAddress;
           }
       }

       /// <summary>
       /// Gets issuer name.
       /// </summary>
       private static string IssuerName
       {
           get
           {
               if (string.IsNullOrEmpty(issuerName))
               {
                   issuerName = ConfigValues.ServiceBusNamespaceIssuerName;
               }

               return issuerName;
           }
       }

       /// <summary>
       /// Gets issuer key.
       /// </summary>
       private static string IssuerKey
       {
           get
           {
               if (string.IsNullOrEmpty(issuerKey))
               {
                   issuerKey = ConfigValues.ServiceBusNamespaceIssuerKey;
               }

               return issuerKey;
           }
       }

       /// <summary>
       /// Gets service bus Uri.
       /// </summary>
       private static Uri ServiceBusUri
       {
           get
           {
               if (serviceBusUri == null)
               {
                   serviceBusUri = ServiceBusEnvironment.CreateServiceUri(ConfigValues.SB, BaseAddress, string.Empty);
               }

               return serviceBusUri;
           }
       }

       /// <summary>
       /// Gets retry policy.
       /// </summary>
       public static RetryPolicy<ServiceBusTransientErrorDetectionStrategy> ServiceBusRetryPolicy
       {
           get
           {
               if (retryPolicy == null)
               {
                   retryPolicy = new RetryPolicy<ServiceBusTransientErrorDetectionStrategy>(ConfigValues.RetryCount, TimeSpan.FromSeconds(ConfigValues.RetryTime));
               }

               return retryPolicy;
           }
       }

       /// <summary>
       /// Gets service bus namespace manager.
       /// </summary>
       private static NamespaceManager ServicebusNamespaceManager
       {
           get
           {
               if (namespaceManager == null)
               {
                   ServiceBusRetryPolicy.ExecuteAction(() =>
                   {
                       namespaceManager = new NamespaceManager(ServiceBusUri, TokenProvider.CreateSharedSecretTokenProvider(IssuerName, IssuerKey));
                   });
               }

               return namespaceManager;
           }
       }

       /// <summary>
       /// Gets messaging factory.
       /// </summary>
       private static MessagingFactory MessagingFactory
       {
           get
           {
               if (messagingFactory == null)
               {
                   ServiceBusRetryPolicy.ExecuteAction(() =>
                   {
                       messagingFactory = MessagingFactory.Create(ServiceBusUri, TokenProvider.CreateSharedSecretTokenProvider(IssuerName, IssuerKey));
                   });
               }

               return messagingFactory;
           }
       }

       /// <summary>
       /// Gets Queue client for queue.
       /// </summary>
       public static QueueClient QueueClient
       {
           get
           {
               QueueDescription queueDescription = null;
               try
               {
                   if (!ServicebusNamespaceManager.QueueExists(QueueName))
                   {
                       queueDescription = ServicebusNamespaceManager.CreateQueue(QueueName);
                   }
                   else
                   {
                       queueDescription = ServicebusNamespaceManager.GetQueue(QueueName);
                   }

                   return MessagingFactory.CreateQueueClient(queueDescription.Path);
               }
               catch(Exception ex)
               {
                   //ff
                   return null;
               }
               
           }
       }

       #endregion
       public static bool CreateQueue(string queuename)
       {
           try
           {
               if (string.IsNullOrEmpty(queueName))
               {
                   Trace.TraceError("ServiceBusRepository : CreateQueue : queueName is empty or null. ");
                   throw new Exception("queue name is null.");
               }

               if (ServicebusNamespaceManager == null)
               {
                   Trace.TraceError("ServiceBusRepository : CreateQueue : ServicebusNamespaceManager is null. ");
                   throw new Exception("Topic is null.");
               }

               if (!ServicebusNamespaceManager.QueueExists(queueName))
               {
                   ServiceBusRetryPolicy.ExecuteAction(() =>
                   {
                       ServicebusNamespaceManager.CreateQueue(queueName);
                   });
               }

               return true;
           }
           catch (Exception ex)
           {
               Trace.TraceError("ServiceBusRepository : CreateQueue : Exception occurred while creating queue: '" + queueName + "' - " + ex.ToString() + " \n Inner Exception - " + ex.InnerException.ToString());
               throw;
           }
 
       }

       /// <summary>
       /// Polls message from queue.
       /// </summary>
       /// <returns>Brokered message.</returns>
       public static BrokeredMessage PollMessageFromQueue()
       {
           try
           {
               if (string.IsNullOrEmpty(QueueName))
               {
                   Trace.TraceError("ServiceBusRepository : PollMessageFromQueue : QueueName is empty or null. ");
                   return null;
               }

               if (MessagingFactory == null)
               {
                   Trace.TraceError("ProcessMessageQueue : PollMessageFromQueue : MessagingFactory is null. ");
                   return null;
               }

               BrokeredMessage message = null;

               ServiceBusRetryPolicy.ExecuteAction(() =>
               {
                   QueueClient queueClient = MessagingFactory.CreateQueueClient(QueueName);
                   message = queueClient.Receive();
               });

               return message;
           }
           catch (Exception ex)
           {
               Trace.TraceError("ProcessMessageQueue  : PollMessageFromQueue : Exception occurred while polling message to from queue: " + QueueName + " - " + ex.ToString() + " \n Inner Exception - " + ex.InnerException.ToString());
               throw new Exception("Error occurred while initializing poll message from queue", ex);
           }
       }

       /// <summary>
       /// Pushes message into the queue.
       /// </summary>
       /// <param name="message">Message to be pushed.</param>
       /// <returns>True if successful else false.</returns>
       public static bool PushMessageToQueue(BrokeredMessage message)
       {
           try
           {
               if (message == null)
               {
                   Trace.TraceError("ProcessMessageQueue : PushMessageToQueue : message is empty or null. ");
                   throw new ArgumentNullException("message");
               }

               if (string.IsNullOrEmpty(QueueName))
               {
                   Trace.TraceError("ProcessMessageQueue : PushMessageToQueue : QueueName is empty or null. ");
                   return false;
               }

               if (MessagingFactory == null)
               {
                   Trace.TraceError("ProcessMessageQueue : PushMessageToQueue : MessagingFactory is null. ");
                   return false;
               }

               ServiceBusRetryPolicy.ExecuteAction(() =>
               {
                   QueueClient.Send(message);
               });

               return true;
           }
           catch (Exception ex)
           {
               Trace.TraceError("ProcessMessageQueue : PushMessageToQueue : Exception occurred while pushing message to the queue: " + QueueName + " - " + ex.ToString() + " \n Inner Exception - " + ex.InnerException.ToString());
               throw new Exception("Error occurred while initializing service bus repository", ex);
           }
       }
      
    }
}
