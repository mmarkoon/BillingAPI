using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BillingAPI.Models.Models
{
    [DataContract]
    public class PaymentGatewayResponseData : BaseGatewayResponse
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
