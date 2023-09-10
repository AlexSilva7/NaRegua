using NaRegua_Api.Common.Enums;

namespace NaRegua_Api.Models.Users
{
    public class DepositInfo
    {
        public decimal Value { get; set; }
        public PaymentType PaymentType { get; set; }
        public string CardNumber { get; set; }
    }
}
