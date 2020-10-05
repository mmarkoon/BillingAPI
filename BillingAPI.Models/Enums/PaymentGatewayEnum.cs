using System.Runtime.Serialization;


namespace BillingAPI.Models.Enums
{
    public enum PaymentGatewayEnum
    {
        [EnumMember(Value = "GPP")]
        GoldenPayPayment = 1,

        [EnumMember(Value = "PPP")]
        PayPalPayment = 2,

        [EnumMember(Value = "BTP")]
        BankTransferPayment = 3,

        [EnumMember(Value = "TST")]
        InvalidGatewayIdForTests = 4
    }
}
