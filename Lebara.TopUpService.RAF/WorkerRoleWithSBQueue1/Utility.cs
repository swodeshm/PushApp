using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRoleWithSBQueue1
{
    public static class Utility
    {
        

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
        private static IN2NewService.GenericINResponse PerformCommissionTopUp(double commisionAmount, string MSISDN, string serialNumber, string countryCode, string currencyCode)
        {
            var ocsServiceClient = new IN2NewService.ServicesClient();
            ocsServiceClient.ClientCredentials.Windows.ClientCredential.UserName = "crmserviceESB";
            ocsServiceClient.ClientCredentials.Windows.ClientCredential.Password = "crm$erv!ce3sb";
            ocsServiceClient.ClientCredentials.Windows.ClientCredential.Domain = "GBR";
            var response = new IN2NewService.GenericINResponse();           
            var msgAcctAdjustment = new IN2NewService.AccountAdjustmentRequestMessage();
            msgAcctAdjustment.AccountAdjustmentRequest = new IN2NewService.AccountAdjustmentRequest();


            //
            string countryCodeConfig = ConfigurationManager.AppSettings[countryCode];
            string[] countrySeparator = new string[] { "," };
            List<string> ROWcountryconfig = new List<string>(countryCodeConfig.Split(countrySeparator, StringSplitOptions.RemoveEmptyEntries));
            
            string mainProductID = ROWcountryconfig[1];
            string accountNumber = ConfigurationManager.AppSettings[mainProductID + "_" + currencyCode];
            //

            try
            {

                msgAcctAdjustment.AccountAdjustmentRequest.Locale = "";

                msgAcctAdjustment.AccountAdjustmentRequest.AdditionalInfo = "AdjustAccount";
                msgAcctAdjustment.AccountAdjustmentRequest = new IN2NewService.AccountAdjustmentRequest();
                msgAcctAdjustment.AccountAdjustmentRequest.OperateType = 2;
                msgAcctAdjustment.AccountAdjustmentRequest.AdditionalInfo = "RAFVoipAdjustment"; //TODO: specify additionalInfo for BI
                msgAcctAdjustment.AccountAdjustmentRequest.RtnerBehaviour = 0;
                msgAcctAdjustment.AccountAdjustmentRequest.MSISDN = "41" + MSISDN;
                msgAcctAdjustment.AccountAdjustmentRequest.TransactionID = "41" + MSISDN + serialNumber;

                msgAcctAdjustment.AccountAdjustmentRequest.ModifyAcctFeeList = new IN2NewService.ModifyAcctFee[1];
                msgAcctAdjustment.AccountAdjustmentRequest.ModifyAcctFeeList[0] = new IN2NewService.ModifyAcctFee();
                //msgAcctAdjustment.AccountAdjustmentRequest.ModifyAcctFeeList[0].AccountType = "2515";
                msgAcctAdjustment.AccountAdjustmentRequest.ModifyAcctFeeList[0].AccountType = accountNumber;
                //msgAcctAdjustment.AdjustAccountRequest.ModifyAcctFeeList[0].ProductID = "1111112219";
                msgAcctAdjustment.AccountAdjustmentRequest.ModifyAcctFeeList[0].Amount = Convert.ToInt64(commisionAmount * 1000000);

                response = ocsServiceClient.AccountAdjustment(msgAcctAdjustment.AccountAdjustmentRequest);

                //if ((response.ResultHeader.ResultDesc.ToUpper().Contains("DUPLICATED") || response.ResultHeader.ResultCode == "405812039"))
                //{

                //    throw new Exception("Unable to charge roaming fee to subscriber " + MSISDN + Environment.NewLine + "Adjustment response: " + response.ResultHeader.ResultDesc + "-" + response.ResultHeader.ResultDesc);
                //}

                return response;                
                
            }
            catch (Exception ex)
            {
                throw ex;               
            }
        }


        private static void PerformPushNotification(double commisionAmount, string MSISDN, string serialNumber, string countryCode, string currencyCode)
        {
            try
            {
                string strFrom = MSISDN;
                var SubScriberBalance = GetAccountInformation(MSISDN);
                string currentBalance = string.Empty;

                decimal finalBalance = 0;
                foreach (IN2NewService.BalanceRecordType subAccount in SubScriberBalance.IntegrationEnquiryResponse.BalanceRecordList)
                {
                    if (subAccount.AccountType.Equals("2000"))
                    {
                        decimal a = subAccount.Balance;
                        currentBalance = System.Math.Round((a / 1000000), 3).ToString();
                    }
                    decimal subaccountBalance = subAccount.Balance;

                    finalBalance += subAccount.Balance;
                }
                //added
                string currCode = currencyCode;
                //
                string finalBalanceString = System.Math.Round((finalBalance / 1000000), 3).ToString();
                string language = "en"; //request.TopUpRequestField.RequestHeader.ITData.Locale.Substring(0, 2);
                string Message = GetNotificationMessage(language);
                if (language == "en" || Message == string.Empty)
                {
                    if (Message == string.Empty)
                    {
                        Message = GetNotificationMessage("en");
                    }
                    //Message = string.Format(Message, currCode, request.TopUpRequestField.Body.pAmount, currCode, finalBalanceString);
                    //added
                    Message = string.Format(Message, currCode, commisionAmount, currCode, finalBalanceString);
                }
                else
                {
                    // Message = string.Format(Message, currCode, request.TopUpRequestField.Body.pAmount, currCode, finalBalanceString);
                    //added
                    Message = string.Format(Message, currCode, commisionAmount, currCode, finalBalanceString);
                }
                var parameters = new Parameters();
                // parameters.to.Add(request.TopUpRequestField.Body.pTargetMSISDN);
                parameters.to.Add(MSISDN);
                parameters.from = strFrom;
                parameters.message = Message;
                var response = PushNotification(parameters);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to Perform PushNotification");
                throw new Exception("Unable to Perform PushNotification " + MSISDN );
            }

        }
        public static string PushNotification(Parameters requestMessage)
        {
            var responseList = RestServiceCalls.SendMessage(requestMessage);
            return responseList;
        }

        public static string GetNotificationMessage(string language)
        {
            using (System.Data.SqlClient.SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Lebara.TopUp.Service.BusinessLogic.Properties.Settings.LebaraTopUpConnectionString"].ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = connection;
                    comm.Connection.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = "sp_GetPushNotificationMessages";
                    comm.Parameters.Add(new SqlParameter() { ParameterName = "Language", Value = language });
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        return reader["PushNotificationMessage"].ToString();
                    }
                }
            }
            return string.Empty;

        }
        public static  IN2NewService.GetAccountInformationResponse GetAccountInformation(string strMSISDN)
        {
            IN2NewService.ServicesClient client = new IN2NewService.ServicesClient();
            IN2NewService.GetAccountInformationRequest objIN2request = new IN2NewService.GetAccountInformationRequest();
            IN2NewService.GetAccountInformationResponse objIN2Response = null;
            objIN2request.MSISDN = strMSISDN;
            objIN2request.QueryType = IN2NewService.INQueryType.SubscriberBalance;
            objIN2Response = client.GetAccountInformation(objIN2request);
            if (objIN2Response.INResponse.Result == IN2NewService.INResult.OK)
            {
                return objIN2Response;
            }
            else { return null; }
        }

        private static void CreateSubscriber(string MSISDN, string StrCountryCode)
        {
            try
            {
                GetAccountInfoAndProvisionSubscriber(MSISDN, StrCountryCode);
            }
 
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private static bool GetAccountInfoAndProvisionSubscriber(string MSISDN, string StrCountryCode)
        {
            try
            {
                var ocsServiceClient = new IN2NewService.ServicesClient();
                ocsServiceClient.ClientCredentials.Windows.ClientCredential.UserName = "crmserviceESB";
                ocsServiceClient.ClientCredentials.Windows.ClientCredential.Password = "crm$erv!ce3sb";
                ocsServiceClient.ClientCredentials.Windows.ClientCredential.Domain = "GBR";
                var getAccountInfoResponse = new IN2NewService.GetAccountInformationResponse();
                var getAccountInfoRequest = new IN2NewService.GetAccountInformationRequest();

                getAccountInfoRequest.MSISDN = "41" + MSISDN;
                getAccountInfoRequest.QueryType = IN2NewService.INQueryType.All;
                getAccountInfoResponse = ocsServiceClient.GetAccountInformation(getAccountInfoRequest);
                if (getAccountInfoResponse != null && getAccountInfoResponse.INResponse != null
                   && getAccountInfoResponse.INResponse.Result != IN2NewService.INResult.OK)
                    //&& getAccountInfoResponse.INResponse.ResultCode != "405000000")
                {
                    string countryCodeConfig = ConfigurationManager.AppSettings[StrCountryCode];
                    string[] countrySeparator = new string[] { "," };
                    List<string> ROWcountryconfig = new List<string>(countryCodeConfig.Split(countrySeparator, StringSplitOptions.RemoveEmptyEntries));
                    string cCode = ROWcountryconfig[0];
                    string crmId = GetCRMId(cCode);
                    IN2NewService.GenericINResponse in2response = new IN2NewService.GenericINResponse();
                    IN2NewService.ServicesClient client = new IN2NewService.ServicesClient();
                    IN2NewService.ProvisionSubscriberRequest request = new IN2NewService.ProvisionSubscriberRequest();
                    //string mainProductIdkey = StrCountryCode + "_MainProductID";
                    //string mainProductID = ConfigurationManager.AppSettings[mainProductIdkey]; 
                    string mainProductID = ROWcountryconfig[1];
                    request.MSISDN = "41" + MSISDN;
                    request.AccountInformation = new IN2NewService.Account();
                    request.AccountInformation.BillCycleType = "1";
                    request.SubscriberInformation = new IN2NewService.SubscriberDetails();
                    request.SubscriberInformation.IVRLanguage = "1";
                    request.SubscriberInformation.MainProductID = mainProductID;
                    request.SubscriberInformation.PayMode = IN2NewService.SubscriberType.Prepaid;
                    request.CustomerID = crmId;
                    in2response = ocsServiceClient.ProvisionSubscriber(request);
                    if (in2response != null && in2response.Result == IN2NewService.INResult.OK)
                    {
                        Activate(MSISDN);
                    }
                }
                else if (getAccountInfoResponse != null && getAccountInfoResponse.INResponse != null && getAccountInfoResponse.IntegrationEnquiryResponse != null
                    && getAccountInfoResponse.IntegrationEnquiryResponse.SubscriberState != null)
                {
                    if (getAccountInfoResponse.IntegrationEnquiryResponse.SubscriberState.LifeCycleState.HasValue)
                    {
                        if (getAccountInfoResponse.IntegrationEnquiryResponse.SubscriberState.LifeCycleState.Value == 1)
                        {
                            Activate(MSISDN);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool Activate(string msisdn)
        {
            try
            {
                var ocsServiceClient = new IN2NewService.ServicesClient();
                ocsServiceClient.ClientCredentials.Windows.ClientCredential.UserName = "crmserviceESB";
                ocsServiceClient.ClientCredentials.Windows.ClientCredential.Password = "crm$erv!ce3sb";
                ocsServiceClient.ClientCredentials.Windows.ClientCredential.Domain = "GBR";
                var activateRequest = new IN2NewService.ActivateRequest();
                var activateResponse = new IN2NewService.GenericINResponse();
                activateRequest.AccessMethod = 5;
                activateRequest.MSISDN = "41" + msisdn;
                activateResponse = ocsServiceClient.Activate(activateRequest);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a new CRM ID
        /// </summary>
        /// <param name="countryCode">Country Code</param>
        /// <returns>New CRM ID.</returns>
        public static string GetCRMId(string countryCode)
        {
            var CRMIDGenerateClient = new GetNewCRMId.CRMServiceClient();
            var CRMIdRequest = new GetNewCRMId.NewCRMIdRequest();
            var response = CRMIDGenerateClient.GenerateNewCRMId(DateTime.Now);
            return "0" + response + countryCode;
        }

         public static string GetSerialNumber()
         {
             return DateTime.Now.Ticks.ToString();
         }


        /// <summary>
        /// FetchRAF Details
        /// </summary>
        /// <param name="ReferredMSISDN">MSISDN</param>
        
        /// <returns>Database Values</returns>
        public static  List<spFetchRAFDetailsResult> fetchRAFDetails(string MSISDN)
        {
            List<spFetchRAFDetailsResult> rafDetails = null;
            using (RAFDataClassDataContext rafDataContext = new RAFDataClassDataContext(ConfigurationManager.ConnectionStrings["WorkerRoleWithSBQueue1.Properties.Settings.LebaraTopUpConnectionString"].ToString()))
            {
                rafDetails = rafDataContext.spFetchRAFDetails(MSISDN).ToList();
                return rafDetails;
            }
        }

        public static int updateRAFDetails(string refereeMSISDN,string refererMSISDN,string referalCode,string TrnsactionID,int? refereeTopupCount,int? refererTopupCount)
        {
            int result = 0;
            using (RAFDataClassDataContext rafDataContext = new RAFDataClassDataContext(ConfigurationManager.ConnectionStrings["WorkerRoleWithSBQueue1.Properties.Settings.LebaraTopUpConnectionString"].ToString()))
            {
                result = rafDataContext.spUpdateRAFDetails(refererMSISDN, refereeMSISDN, referalCode, TrnsactionID, refererTopupCount, refereeTopupCount);
                return result;
            }
        }

        public static void InsertRAFRequestAll(string ReferrerMSISDN, string ReferreeMSISDN, string ReferrerCurrency,
            string ReferrerCountryCode, string ReferreeCountryCode, string ReferreeCurrency, string ReferalCode, int? refererTopupCount, int? refereeTopupCount, decimal? refererAwardedAmount, decimal? refereeAwardedAmount,bool isAdjustmentSuccessful,string status,string transactionID)
        {
            using (RAFDataClassDataContext rafDataContext = new RAFDataClassDataContext(ConfigurationManager.ConnectionStrings["WorkerRoleWithSBQueue1.Properties.Settings.LebaraTopUpConnectionString"].ToString()))
            {
                rafDataContext.sp_InsertRAFAllTransaction(ReferrerMSISDN, ReferrerCountryCode, ReferrerCurrency, refererTopupCount, ReferreeMSISDN, ReferreeCountryCode, ReferreeCurrency, ReferalCode, refereeTopupCount, (System.Decimal)refereeAwardedAmount, (System.Decimal)refererAwardedAmount, System.DateTime.Now, isAdjustmentSuccessful, status, "Voip", transactionID, "PAYG");
            }
        }



        public static  double calculatePromotionAmount(double refereeToppedUpAmount,string refererCurrency,string refereeCurrency)
        {
            double partialTopupAmount = refereeToppedUpAmount * 0.1;
            partialTopupAmount = (partialTopupAmount * Convert.ToDouble(ConfigurationManager.AppSettings[refereeCurrency]))/Convert.ToDouble(ConfigurationManager.AppSettings[refererCurrency]);
            return partialTopupAmount;
        }


        public static bool ApplyPromotion(string MSISDN,double refereeToppedUpAmount,string serialNumber)
        {
            List<spFetchRAFDetailsResult> rafDetails = null;
            var response = new IN2NewService.GenericINResponse();
            rafDetails = fetchRAFDetails(MSISDN);
            int ReferreeTopupCount = 0;
            int ReferrerTopupCount = 0;
            string adjustedMSISDN = string.Empty;
            try
            {
                if (rafDetails.Count > 0)
                {
                    if (rafDetails[0].ReferreeTopupCount == 0)
                    {
                        ReferreeTopupCount = rafDetails[0].ReferreeTopupCount;
                        GetAccountInfoAndProvisionSubscriber(rafDetails[0].ReferreeMSISDN, rafDetails[0].ReferreeCountryCode);
                        response = PerformCommissionTopUp(Convert.ToDouble(ConfigurationManager.AppSettings["RefereeFirstTopupAmount_" + rafDetails[0].ReferreeCurrency]), adjustedMSISDN = MSISDN, serialNumber + "1", rafDetails[0].ReferreeCountryCode, rafDetails[0].ReferreeCurrency);
                        if (response.Result == IN2NewService.INResult.OK)
                        {
                            ReferreeTopupCount = rafDetails[0].ReferreeTopupCount + 1;
                            PerformPushNotification(Convert.ToDouble(ConfigurationManager.AppSettings["RefererFirstTopupAmount_" + rafDetails[0].ReferrerCurrency]), adjustedMSISDN = rafDetails[0].ReferrerMSISDN, serialNumber + "2", rafDetails[0].ReferrerCountryCode, rafDetails[0].ReferrerCurrency);
                        }
                        updateRAFDetails(rafDetails[0].ReferreeMSISDN, adjustedMSISDN = rafDetails[0].ReferrerMSISDN, rafDetails[0].ReferalCode, adjustedMSISDN + serialNumber + "3", ReferreeTopupCount, rafDetails[0].ReferrerTopupCount);
                        InsertRAFRequestAll(rafDetails[0].ReferrerMSISDN, rafDetails[0].ReferreeMSISDN, rafDetails[0].ReferrerCurrency, rafDetails[0].ReferrerCountryCode, rafDetails[0].ReferreeCountryCode, rafDetails[0].ReferreeCurrency,
                                                rafDetails[0].ReferalCode, (int?)rafDetails[0].ReferrerTopupCount, (int?)rafDetails[0].ReferreeTopupCount + 1, (decimal?)0.0, (decimal?)Convert.ToDouble(ConfigurationManager.AppSettings["RefereeFirstTopupAmount"]),
                                                true, response.ResultCode, adjustedMSISDN + serialNumber + "1");
                    }
                    if (rafDetails[0].ReferrerTopupCount == 0)
                    {
                        CreateSubscriber(rafDetails[0].ReferrerMSISDN, rafDetails[0].ReferrerCountryCode);
                        ReferrerTopupCount = rafDetails[0].ReferrerTopupCount;
                        response = PerformCommissionTopUp(Convert.ToDouble(ConfigurationManager.AppSettings["RefererFirstTopupAmount_" + rafDetails[0].ReferrerCurrency]), adjustedMSISDN = rafDetails[0].ReferrerMSISDN, serialNumber + "2", rafDetails[0].ReferrerCountryCode, rafDetails[0].ReferrerCurrency);
                        if (response.Result == IN2NewService.INResult.OK)
                        {
                            ReferrerTopupCount = rafDetails[0].ReferrerTopupCount + 1;
                            PerformPushNotification(Convert.ToDouble(ConfigurationManager.AppSettings["RefererFirstTopupAmount_" + rafDetails[0].ReferrerCurrency]), adjustedMSISDN = rafDetails[0].ReferrerMSISDN, serialNumber + "2", rafDetails[0].ReferrerCountryCode, rafDetails[0].ReferrerCurrency);
                        }
                        updateRAFDetails(rafDetails[0].ReferreeMSISDN, rafDetails[0].ReferrerMSISDN, rafDetails[0].ReferalCode, adjustedMSISDN + serialNumber + "3", rafDetails[0].ReferreeTopupCount == 0 ? ReferreeTopupCount : rafDetails[0].ReferreeTopupCount, ReferrerTopupCount);
                        InsertRAFRequestAll(rafDetails[0].ReferrerMSISDN, rafDetails[0].ReferreeMSISDN, rafDetails[0].ReferrerCurrency, rafDetails[0].ReferrerCountryCode, rafDetails[0].ReferreeCountryCode, rafDetails[0].ReferreeCurrency,
                                                rafDetails[0].ReferalCode, (int?)rafDetails[0].ReferrerTopupCount + 1, (int?)rafDetails[0].ReferreeTopupCount == 0 ? ReferreeTopupCount : rafDetails[0].ReferreeTopupCount, (decimal?)Convert.ToDouble(ConfigurationManager.AppSettings["RefererFirstTopupAmount"]), (decimal?)0.0,
                                                true, response.ResultCode, adjustedMSISDN + serialNumber + "2");
                    }
                    else if (rafDetails[0].ReferrerTopupCount > 0 && rafDetails[0].ReferrerTopupCount <= Convert.ToInt32(ConfigurationManager.AppSettings["RefererMaxTopupCount"]))
                    {
                        CreateSubscriber(rafDetails[0].ReferrerMSISDN, rafDetails[0].ReferrerCountryCode);
                        ReferrerTopupCount = rafDetails[0].ReferrerTopupCount;
                        double topupAmount = calculatePromotionAmount(refereeToppedUpAmount, rafDetails[0].ReferrerCurrency, rafDetails[0].ReferreeCurrency);
                        response = PerformCommissionTopUp(topupAmount, adjustedMSISDN = rafDetails[0].ReferrerMSISDN, serialNumber + "3", rafDetails[0].ReferrerCountryCode, rafDetails[0].ReferrerCurrency);
                        if (response.Result == IN2NewService.INResult.OK)
                        {
                            ReferrerTopupCount = rafDetails[0].ReferrerTopupCount + 1;
                        }
                        updateRAFDetails(rafDetails[0].ReferreeMSISDN, rafDetails[0].ReferrerMSISDN, rafDetails[0].ReferalCode, adjustedMSISDN + serialNumber + "3", rafDetails[0].ReferreeTopupCount == 0 ? ReferreeTopupCount : rafDetails[0].ReferreeTopupCount, ReferrerTopupCount);
                        InsertRAFRequestAll(rafDetails[0].ReferrerMSISDN, rafDetails[0].ReferreeMSISDN, rafDetails[0].ReferrerCurrency, rafDetails[0].ReferrerCountryCode, rafDetails[0].ReferreeCountryCode, rafDetails[0].ReferreeCurrency,
                                                rafDetails[0].ReferalCode, (int?)rafDetails[0].ReferrerTopupCount + 1, (int?)rafDetails[0].ReferreeTopupCount == 0 ? ReferreeTopupCount : rafDetails[0].ReferreeTopupCount, (decimal?)topupAmount, (decimal?)0.0,
                                                true, response.ResultCode, adjustedMSISDN + serialNumber + "3");
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return true;
        }
    }
  
}
