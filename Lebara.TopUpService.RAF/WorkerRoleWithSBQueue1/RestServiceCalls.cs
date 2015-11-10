using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRoleWithSBQueue1
{
    public class RestServiceCalls
    {
        public static string SendMessage(Parameters requestMessage)
        {
            string url = ConfigurationManager.AppSettings["PNURL"]; ;
            using (var client = new RestClient(url, Enums.HttpVerb.POST, requestMessage))
            {
                try
                {
                    string responseStatus = client.InvokeService();
                    return responseStatus;
                }
                catch (WebException ex1)
                {
                    Trace.TraceError("Exception : SendMessage : Exception occurred while SendMessage. - " + ex1.ToString() + " \n Inner Exception - " + ex1.InnerException.ToString());
                    return null;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception : SendMessage : Exception occurred while SendMessage. - " + ex.ToString() + " \n Inner Exception - " + ex.InnerException.ToString());
                    return null;
                }
            }
        }

    }
}