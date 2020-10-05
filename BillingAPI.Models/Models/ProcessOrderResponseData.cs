using System.Runtime.Serialization;

namespace BillingAPI.Models.Models
{
    [DataContract]
    public class ProcessOrderResponseData
    {
        [DataMember(Name = "receiptdata")]
        public ReceiptData ReceiptData { get; set; }

        [DataMember(Name = "orderNumber")]
        public string OrderNumber { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        [DataMember(Name = "paymentAccepted")]
        public bool PaymentAccepted { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
