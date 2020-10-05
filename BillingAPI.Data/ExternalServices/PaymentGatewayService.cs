using BillingAPI.Models.Enums;
using BillingAPI.Models.Models;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;

namespace BillingAPI.Data.ExternalServices
{
    public interface IPaymentGatewayService
    {
        ReceiptData ProcessPaymentData(ProcessOrderRequestData orderData);
    }

    public class PaymentGatewayService : IPaymentGatewayService
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly JsonMediaTypeFormatter[] formatters =
        {
            new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                    {
                        new StringEnumConverter()
                    },
                    NullValueHandling = NullValueHandling.Ignore
                }
            }
        };

        public ReceiptData ProcessPaymentData(ProcessOrderRequestData orderData)
        {
            switch (orderData.PaymentGateway)
            {
                case PaymentGatewayEnum.BankTransferPayment:
                    return SendBtpPayment(orderData);
                case PaymentGatewayEnum.GoldenPayPayment:
                    return SendGppPayment(orderData);
                case PaymentGatewayEnum.PayPalPayment:
                    return SendPppPayment(orderData);
                default:
                    throw new InvalidOperationException($"Payment gateway type {orderData.PaymentGateway} is invalid!");
            }
        }

        private ReceiptData SendPppPayment(ProcessOrderRequestData orderData)
        {
            CreatePaymentDraftData(orderData.OrderNumber, orderData.PayableAmount);

            //Do some other payment gateway specific actions here

            var url = ConfigurationManager.AppSettings["PppGatewayUrl"];
            var result = GetPaymentGatewayResponse<ProcessOrderRequestData, PaymentGatewayResponseData>(orderData, url);

            if(result != null)
            {
                var receiptData = new ReceiptData
                {
                    ReceiptNumber = result.ReceiptNumber,
                    PaymentNumber = result.PaymentNumber,
                    AmountPaid = result.AmountPaid,
                    DateProcessed = result.DateProcessed
                };

                UpdatePaymentDraftData(receiptData);
                return receiptData;
            }

            return null;
        }

        private ReceiptData SendGppPayment(ProcessOrderRequestData orderData)
        {
            CreatePaymentDraftData(orderData.OrderNumber, orderData.PayableAmount);

            //Do some other payment gateway specific actions here

            var url = ConfigurationManager.AppSettings["GppGatewayUrl"];
            var result = GetPaymentGatewayResponse<ProcessOrderRequestData, PaymentGatewayResponseData>(orderData, url);

            if (result != null)
            {
                var receiptData = new ReceiptData
                {
                    ReceiptNumber = result.ReceiptNumber,
                    PaymentNumber = result.PaymentNumber,
                    AmountPaid = result.AmountPaid,
                    DateProcessed = result.DateProcessed
                };

                UpdatePaymentDraftData(receiptData);
                return receiptData;
            }

            return null;
        }

        private ReceiptData SendBtpPayment(ProcessOrderRequestData orderData)
        {
            CreatePaymentDraftData(orderData.OrderNumber, orderData.PayableAmount);

            //Do some other payment gateway specific actions here
            
            var url = ConfigurationManager.AppSettings["BtpGatewayUrl"];
            var result = GetPaymentGatewayResponse<ProcessOrderRequestData, PaymentGatewayResponseData>(orderData, url);

            if (result != null)
            {
                var receiptData = new ReceiptData
                {
                    ReceiptNumber = result.ReceiptNumber,
                    PaymentNumber = result.PaymentNumber,
                    AmountPaid = result.AmountPaid,
                    DateProcessed = result.DateProcessed
                };

                UpdatePaymentDraftData(receiptData);
                return receiptData;
            }

            return null;
        }

        private void CreatePaymentDraftData(string orderNumber, decimal payableAmount)
        {
            //Do business logic here, for example,
            //save payment draft data in database, etc.
        }

        private void UpdatePaymentDraftData(ReceiptData receiptData)
        {
            //Do business logic here, for example,
            //update payment draft data in database (change status, set received values), etc.
        }

        private U GetPaymentGatewayResponse<T, U>(T request, string url) where U : BaseGatewayResponse
        {
            U responseResult = null;
            var methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                using (var client = new HttpClient())
                {
                    Logger.Info($"Method: {methodName}, Request: {request}, Url: {url}");
                    var response = client.PostAsJsonAsync(url, request).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        Logger.Info($"Method: {methodName}, Response: {response}, Url: {url}");
                        return null;
                    }
                    var test = response.Content.ReadAsStringAsync().Result;
                    responseResult = response.Content.ReadAsAsync<U>(formatters).Result;

                    var json = JsonConvert.SerializeObject(responseResult);
                    Logger.Info($"Method: {methodName}, Response: {response}, Url: {url}");
                }
            }
            catch (Exception e)
            {
                Logger.Error($"Method: {methodName}, Exception: {e.Message}");
            }

            return responseResult;
        }
    }
}
