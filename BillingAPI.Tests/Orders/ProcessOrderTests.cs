using BillingAPI.Data.ExternalServices;
using BillingAPI.Data.Operations;
using BillingAPI.Models.Enums;
using BillingAPI.Models.Models;
using BillingAPI.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BillingAPI.Tests.Orders
{
    [TestClass()]
    public class ProcessOrderTests
    {
        private readonly IProcessOrderOperation processOrderOperation;
        private readonly Mock<IPaymentGatewayService> paymentGatewayServiceMock;

        public ProcessOrderTests()
        {
            paymentGatewayServiceMock = new Mock<IPaymentGatewayService>();
            processOrderOperation = new ProcessOrderOperation(
                paymentGatewayServiceMock.Object
            );
        }


        [TestMethod()]
        public void ProcessOrder_ValidOrder_OrderResponse()
        {
            //Arrange
            var orderData = new OrderRequestDataBuilder().Build();
            SetupProcessPaymentData(orderData);

            //Act
            var orderResponse = processOrderOperation.ProcessOrder(orderData);

            //Assert
            paymentGatewayServiceMock.Verify(x => x.ProcessPaymentData(It.IsAny<ProcessOrderRequestData>()), Times.Once);
            Assert.AreEqual(orderData.OrderNumber, orderResponse.OrderNumber);
            Assert.AreEqual(orderData.UserId, orderResponse.UserId);
            Assert.IsNotNull(orderResponse.ReceiptData);
        }

        [TestMethod()]
        public void ProcessOrder__InvalidPaymentGateway_ThrowsException()
        {
            //Arrange
            var orderData = new OrderRequestDataBuilder().Build();
            orderData.PaymentGateway = PaymentGatewayEnum.InvalidGatewayIdForTests;
            SetupProcessPaymentData(orderData);

            //Act and assert
            Assert.ThrowsException<InvalidOperationException>(() => processOrderOperation.ProcessOrder(orderData));
        }

        [TestMethod()]
        public void ProcessOrder__GatewayServiceFailure_OrderResponseWithErrorMessage()
        {
            //Arrange
            var orderData = new OrderRequestDataBuilder().Build();
            SetupMock(orderData, null);

            //Act
            var orderResponse = processOrderOperation.ProcessOrder(orderData);

            //Assert
            Assert.IsNotNull(orderResponse.ErrorMessage);
            Assert.IsNotNull(orderResponse.OrderNumber);
            Assert.IsNotNull(orderResponse.UserId);
            Assert.IsNull(orderResponse.ReceiptData);
            Assert.IsFalse(orderResponse.PaymentAccepted); 
        }

        private void SetupProcessPaymentData(ProcessOrderRequestData orderData)
        {
            var receiptData = new ReceiptDataBuilder().Build();
            receiptData.AmountPaid = orderData.PayableAmount;
            SetupMock(orderData, receiptData);
        }

        private void SetupMock(ProcessOrderRequestData orderData, ReceiptData receiptData)
        {
            paymentGatewayServiceMock
                .Setup(s => s.ProcessPaymentData(orderData))
                .Returns(receiptData);
        }
    }
}
