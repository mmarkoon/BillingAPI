using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BillingAPI.Models.Models
{
    [DataContract]
    public class BaseGatewayResponse
    {
        [DataMember(Name = "transactionNumber")]
        public string TransactionNumber { get; set; }
    }
}
