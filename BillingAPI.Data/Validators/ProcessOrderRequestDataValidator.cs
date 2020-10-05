using BillingAPI.Models.Enums;
using BillingAPI.Models.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingAPI.Data.Validators
{
    class ProcessOrderRequestDataValidator : AbstractValidator<ProcessOrderRequestData>
    {
        private const string orderNumRegEx = "^(ON)[0-9]{6}$";
        private const string userIdRegEx = "^(ID)[0-9]{4}[A-Z]{2}$";

        private readonly IList<PaymentGatewayEnum> allowedPaymentGateways =
            new List<PaymentGatewayEnum>{
                PaymentGatewayEnum.GoldenPayPayment,
                PaymentGatewayEnum.BankTransferPayment,
                PaymentGatewayEnum.PayPalPayment
            };

        public ProcessOrderRequestDataValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty()
                .Matches(orderNumRegEx);
            RuleFor(x => x.UserId)
                .NotEmpty()
                .Matches(userIdRegEx);
            RuleFor(x => x.PaymentGateway)
                .NotEmpty()
                .Must(v => allowedPaymentGateways.Contains(v));
            RuleFor(x => x.OptionalDescription)
                .NotEmpty();
            RuleFor(x => x.PayableAmount)
                .NotEmpty();
        }
    }
}
