
using BillingAPI.Models.Enums;
using BillingAPI.Models.Models;
using System;

namespace BillingAPI.Tests.TestSupport
{
    internal class ReceiptDataBuilder
    {
        private readonly ReceiptData receiptData;

        public ReceiptDataBuilder()
        {
            receiptData = new ReceiptData
            {
                ReceiptNumber = "RN203309",
                PaymentNumber = "PN276495",
                DateProcessed = DateTime.Now
            };
        }

        public ReceiptData Build()
        {
            return receiptData;
        }
    }
}
