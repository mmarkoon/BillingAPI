using BillingAPI.Data.ExternalServices;
using BillingAPI.Models.Enums;
using BillingAPI.Models.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BillingAPI.Data.Operations
{
    public interface IProcessOrderOperation
    {
        ProcessOrderResponseData ProcessOrder(ProcessOrderRequestData orderData);
    }

    public class ProcessOrderOperation : IProcessOrderOperation
    {
        private readonly IPaymentGatewayService paymentGatewayService;

        public ProcessOrderOperation(IPaymentGatewayService paymentGatewayService)
        {
            this.paymentGatewayService = paymentGatewayService;
        }

        public ProcessOrderResponseData ProcessOrder(ProcessOrderRequestData orderData)
        {
            CreateOrderDraftData(orderData);

            var receipt = paymentGatewayService.ProcessPaymentData(orderData);

            var response = CreateDefaultResponsedata(orderData.OrderNumber, orderData.UserId); 

            if (receipt != null)
            {
                response.PaymentAccepted = true;
                response.ReceiptData = receipt;

                UpdateOrderDraftData(receipt);
            }
            else
            {
                response.PaymentAccepted = false;
                response.ErrorMessage =
                    $"Payment gateway {orderData.PaymentGateway} was unable to process payment for order number {orderData.OrderNumber}. " +
                    $"For detailed information please check error logs";
            }

            return response;
        }

        private ProcessOrderResponseData CreateDefaultResponsedata(string orderNumber, string userId)
        {
            return new ProcessOrderResponseData
            {
                OrderNumber = orderNumber,
                UserId = userId
            };
        }

        private void CreateOrderDraftData(ProcessOrderRequestData orderData)
        {
            //Added gateway validation, basically only for tests, because this case is handled earlier by FluentValidation
            var isGatewayIdValid = 
                new List<PaymentGatewayEnum>
                {
                    PaymentGatewayEnum.GoldenPayPayment,
                    PaymentGatewayEnum.BankTransferPayment,
                    PaymentGatewayEnum.PayPalPayment
                }
                .Contains(orderData.PaymentGateway);

            if (!isGatewayIdValid)
            {
                throw new InvalidOperationException($"Payment gateway type {orderData.PaymentGateway} is invalid!");
            }

            //Do some business logic here before sending to payment gateway,
            //for example, save order data as a draft to database with payment status "Unpaid", etc
        }

        private void UpdateOrderDraftData(ReceiptData receipt)
        {
            //Do some business logic here after receiving response from payment gateway,
            //for example, update order data change status to "Paid", etc
        }
    }
}
