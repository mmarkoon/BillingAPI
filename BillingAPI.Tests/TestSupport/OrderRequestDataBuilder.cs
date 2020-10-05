using BillingAPI.Models.Enums;
using BillingAPI.Models.Models;
using FluentValidation;

namespace BillingAPI.Tests.TestSupport
{
    internal class OrderRequestDataBuilder
    {
        private readonly ProcessOrderRequestData orderData;

        public OrderRequestDataBuilder()
        {
            orderData = new ProcessOrderRequestData
            {
                OrderNumber = "ON285345",
                UserId = "ID3728AI",
                PayableAmount = 100M,
                PaymentGateway = PaymentGatewayEnum.GoldenPayPayment,
                OptionalDescription = "Order weight = 3.14"

            };
        }

        public ProcessOrderRequestData Build()
        {
            return orderData;
        }
    }
}
