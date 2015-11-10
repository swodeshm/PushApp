using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lebara.TopUpService.RAF.Service
{
    public static class ConfigValues
    {

        public const string SB = "sb";

        /// <summary>
        /// Number of times connection should be retried.
        /// </summary>
        public const int RetryCount = 3;

        /// <summary>
        /// Retry time.
        /// </summary>
        public const int RetryTime = 2;

        /// <summary>
        /// waiting time.
        /// </summary>
        public const int WaitingTime = 10000;

        public static string QueueName
        {
            get
            {
                return "rowwo";
            }
        }

        public static string ServiceBusNamespaceAddress 
        {
            get
            {
                return "lebaramessagehandler";
            }
        }

        public static string ServiceBusNamespaceIssuerName
        {
            get
            {
                return "owner";
            }
        }

        public static string ServiceBusNamespaceIssuerKey
        {
            get
            {
                return "t8/vI9RXMZAsKm0R/nWKUPe/UTcn4KIpXy+GBdzCPAI=";
            }
        }
    }
}
