using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Lebara.IN.BossC4BContracts;

namespace Lebara.TopUpService.RAF.Service
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            Trace.TraceInformation("Lebara.TopUpService.RAF.Service entry point called", "Information");
            BrokeredMessage brokeredMessage = null;
            while (true)
            {
                try
                {
                    //var messageTemplate = new WorkOrderRequest();
                    //messageTemplate.WorkOrderType = 72;
                    //messageTemplate.SubscriberNo = "447587480538";
                    //messageTemplate.SerialNo = DateTime.UtcNow.ToShortTimeString();
                    //messageTemplate.ParaList = new WorkOrderRequestParaItem[1];
                    //messageTemplate.ParaList[0] = new WorkOrderRequestParaItem();
                    //messageTemplate.ParaList[0].ParaName = "Text";
                    //messageTemplate.ParaList[0].ParaValue = "Blah Blah Blah";
                    //messageTemplate.IMSI = string.Empty;
                    //messageTemplate.LowBalAcctList = new WorkOrderRequestLowBalAcct[1];
                    //messageTemplate.LowBalAcctList[0] = new WorkOrderRequestLowBalAcct();
                    //messageTemplate.LowBalAcctList[0].AccountType = "Text";
                    //messageTemplate.LowBalAcctList[0].Balance = 0;
                    //messageTemplate.LowBalAcctList[0].BalanceThreshod = 0;
                    //messageTemplate.ProductFeeList = new WorkOrderRequestProductFee[1];
                    //messageTemplate.ProductFeeList[0] = new WorkOrderRequestProductFee();
                    //messageTemplate.ProductFeeList[0].ChargedAmt = 0;
                    //messageTemplate.ProductFeeList[0].ChargeType = string.Empty;
                    //messageTemplate.ProductFeeList[0].FeeID = 0;
                    //messageTemplate.ProductFeeList[0].FeeIDSpecified = false;
                    //messageTemplate.ProductFeeList[0].MinMeasureId = 0;
                    //messageTemplate.ProductFeeList[0].MinMeasureIdSpecified = false;
                    //messageTemplate.ProductOrderInfoList = new WorkOrderRequestProductOrderInfo[1];
                    //messageTemplate.ProductOrderInfoList[0] = new WorkOrderRequestProductOrderInfo();
                    //messageTemplate.ProductOrderInfoList[0].AutoType = 0;
                    //messageTemplate.ProductOrderInfoList[0].EffectiveDate = " ";
                    //messageTemplate.ProductOrderInfoList[0].ExpireDate = " ";
                    //messageTemplate.ProductOrderInfoList[0].ProductID = " ";
                    //messageTemplate.ProductOrderInfoList[0].ProductOrderKey = " ";
                    //messageTemplate.ProductID = " ";
                    //messageTemplate.ProductOrderKey = 0;
                    //messageTemplate.ProductOrderKeySpecified = false;
                    //messageTemplate.SubscriptionExpireTime = " ";
                    //messageTemplate.WorkOrderType = 0;


                    //brokeredMessage = new BrokeredMessage(messageTemplate, new DataContractSerializer(typeof(WorkOrderRequest)));
                    //ProcessMessageQueue.PushMessageToQueue(brokeredMessage);
                    //The above section will be already implemented.A queue will be already formed and pushed a message.Need to remove above lines when its done by Roberto Gonzalez
                    var queueClient = ProcessMessageQueue.QueueClient;
                    brokeredMessage = ProcessMessageQueue.PollMessageFromQueue();

                    //string rawXml = new StreamReader(brokeredMessage.GetBody<Stream>(),Encoding.UTF8).ReadToEnd();
                    //XmlDocument xmlDoc = new XmlDocument();
                    //xmlDoc.LoadXml(rawXml);
                    //var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    //nsmgr.AddNamespace("x", "http://schemas.datacontract.org/2004/07/Lebara.IN.BossC4BContracts");
                    //var nodes = xmlDoc.DocumentElement.SelectNodes("//x:paraValueField", nsmgr);

                    //string paraValue = nodes.Item(0).InnerText;
                    //nodes = xmlDoc.DocumentElement.SelectNodes("//x:subscriberNoField", nsmgr);
                    //string refereeMSISIDN = nodes.Item(0).InnerText;
                    WorkOrderRequest request = brokeredMessage.GetBody<Lebara.IN.BossC4BContracts.WorkOrderRequest>();
                    var topUpAmount = 10;
                    double commissionAmount = Convert.ToDouble(topUpAmount) / 10;
                    
                    //Check existance of MSISDN record in RAFAllRequest table.
                    using (var connection = new SqlConnection(CloudConfigurationManager.GetSetting("ConnectionString"))) //ConfigurationManager.AppSettings["ConnectionString"]);
                    {
                        using (var command = new SqlCommand("select count(*) from RAFAllRequest where ReferreeMSISDN = " + request.SubscriberNo, connection))
                        {
                            connection.Open();
                            int count = (Int32)command.ExecuteScalar();
                            connection.Close();
                            if (count > 0)
                            {
                                string currency = string.Empty;
                                string transactionId = string.Empty;
                                string paymentMethod = string.Empty;
                                string productId = string.Empty;
                                string country = string.Empty;
                                string accountType = string.Empty;
                                string locale = string.Empty;
                                var response = PerformCommissionTopUp(commissionAmount, request.SubscriberNo, request.ParaList[0].ParaValue, country, accountType, locale);
                                if (response != null && response.Result.ToString().Equals("OK", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    brokeredMessage.Complete();
                                    //Here an entry should go RAFAllRequestDetails
                                    //
                                    connection.Open();
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.CommandText = "GetRefereeDetails";
                                    command.Parameters.Add("@MSISDN", SqlDbType.VarChar, 50);
                                    command.Parameters["@MSISDN"].Value = request.SubscriberNo;
                                    command.Connection = connection;
                                    var status = command.ExecuteNonQuery();
                                    connection.Close();
                                    //Here an update should go to RAFAllRequest table ReferreeTopupCount increment by 1.
                                    //
                                    Trace.TraceInformation("Top Up Sucess", response.Result);
                                }
                                else
                                {
                                    brokeredMessage.Abandon();
                                    Trace.TraceInformation("Top Up Failed", response.ErrorMessage);
                                }
                            }
                        }
                    }
                    
                    Thread.Sleep(ConfigValues.WaitingTime);
                }
                catch(Exception ex)
                {
                    Trace.TraceInformation("Exception occurred", "Exception Lebara.TopUpService.RAF.Service" + ex.Message);
                    brokeredMessage.Abandon();
                }
            }
        }


        /// <summary>
        /// This is to topup the commission percentage.
        /// </summary>
        /// <param name="commisionAmount">coomission amount</param>
        /// <param name="MSISDN">mobile number.</param>
        /// <param name="paravalue">The textual value for transaction.</param>
        /// <param name="currency">The currency</param>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="paymentMethod">The payment option method</param>
        /// <param name="productId">Product Id.</param>
        /// <returns>The top up response with error codes.</returns>
        private IN2NewService.GenericINResponse PerformCommissionTopUp(double commisionAmount, string MSISDN, string paravalue, string country, string accountType, string locale)
        {
            var ocsServiceClient = new IN2NewService.ServicesClient();
            ocsServiceClient.ClientCredentials.Windows.ClientCredential.UserName = "crmserviceESB";
            ocsServiceClient.ClientCredentials.Windows.ClientCredential.Password = "crm$erv!ce3sb";
            ocsServiceClient.ClientCredentials.Windows.ClientCredential.Domain = "GBR";
            var response = new IN2NewService.GenericINResponse();
            var accountAdjustReq = new IN2NewService.AccountAdjustmentRequest();
            try
            {
                accountAdjustReq.AdditionalInfo = "UK";
                accountAdjustReq.Locale = "en-GB";
                accountAdjustReq.MSISDN = MSISDN;
                accountAdjustReq.RtnerBehaviour = 1;
                accountAdjustReq.ModifyAcctFeeList = new IN2NewService.ModifyAcctFee[1];
                accountAdjustReq.ModifyAcctFeeList[0] = new IN2NewService.ModifyAcctFee();
                accountAdjustReq.ModifyAcctFeeList[0].AccountType = "2000";
                accountAdjustReq.ModifyAcctFeeList[0].Amount = Convert.ToInt32(commisionAmount * 1000000);
                response = ocsServiceClient.AccountAdjustment(accountAdjustReq);
                return response;
            }
            catch(Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// On start event.
        /// </summary>
        /// <returns></returns>
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
