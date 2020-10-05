using System;
using System.Runtime.Serialization;

namespace BillingAPI.Models.Models
{
    [DataContract]
    public class ReceiptData
    {
        [DataMember(Name = "receiptNumber")]
        public string ReceiptNumber { get; set; }

        [DataMember(Name = "paymentNumber")]
        public string PaymentNumber { get; set; }

        [DataMember(Name = "amountPaid")]
        public decimal AmountPaid { get; set; }

        [DataMember(Name = "dateProcessed")]
        public DateTime DateProcessed { get; set; }
    }
}
