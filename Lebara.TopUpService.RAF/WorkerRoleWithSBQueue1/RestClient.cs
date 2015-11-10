using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRoleWithSBQueue1
{
    public class RestClient : IDisposable
    {
        public string EndPoint { get; set; }
        public Enums.HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public Parameters PostData { get; set; }
        public string Accept { get; set; }
        public RestClient()
        {
            EndPoint = "";
            Method = Enums.HttpVerb.GET;
            Accept = "application/x-www-form-urlencoded";
            //ContentType = "text/xml";
            ContentType = "application/x-www-form-urlencoded";
            //PostData = "";
        }
        public RestClient(string endpoint)
        {
            EndPoint = endpoint;
            Method = Enums.HttpVerb.GET;
            Accept = "application/json";

            //ContentType = "text/xml";
            ContentType = "application/json";
            //PostData = "";
        }
        public RestClient(string endpoint, Enums.HttpVerb method)
        {
            EndPoint = endpoint;
            Accept = "application/json";
            Method = method;
            //ContentType = "text/xml";
            ContentType = "application/json";
            //PostData = "";
        }

        public RestClient(string endpoint, Enums.HttpVerb method, Parameters postData)
        {
            EndPoint = endpoint;
            Accept = "application/json";
            Method = method;
            ContentType = "application/json";
            PostData = postData;
        }

        public string InvokeService()
        {
            return InvokeService(string.Empty);
        }

        public string InvokeService(string requestMessage)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                string base64 = ConfigurationManager.AppSettings["PNBase64String"];
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add("Authorization", "Basic " + base64);
                try
                {
                    var JsonString = JsonConvert.SerializeObject(this.PostData);
                    //client.Credentials = new System.Net.NetworkCredential("notifyServiceUser-nUPLw93hsmEJp", "i025Yz4WjW%9Eewq(4f#Q4O7x03)SSs3");
                    var responsebytes = client.UploadString(this.EndPoint, JsonString);
                    switch (responsebytes)
                    {
                        case "204 No Content":
                            return "No Content";
                        case "4xx":
                            return "You did something wrong.";
                        case "403 Forbidden":
                            return "The given authorization credentials are invalid.";
                        case "404 Not Found":
                            return "The given to or from users are not known in the system, the message has not been sent.";
                        case "5xx":
                            return "Something went wrong on the server";
                        case "":
                            return "";
                    }

                }
                catch (WebException e)
                {
                    return "SE";
                }
                catch (Exception ex)
                {
                    return "SE";
                }
                return "SE";
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RestClient()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

        }

    }
}
