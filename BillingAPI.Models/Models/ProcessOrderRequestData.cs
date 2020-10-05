using BillingAPI.Models.Enums;
using System.Runtime.Serialization;

namespace BillingAPI.Models.Models
{
    [DataContract]
    public class ProcessOrderRequestData
    {
        [DataMember(Name = "orderNumber")]
        public string OrderNumber { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        [DataMember(Name = "payableAmount")]
        public decimal PayableAmount { get; set; }

        [DataMember(Name = "paymentGateway")]
        public PaymentGatewayEnum PaymentGateway { get; set; }

        [DataMember(Name = "optionalDescription")]
        public string OptionalDescription { get; set; }
    }
}
