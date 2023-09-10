using NaRegua_Api.Common.Enums;

namespace NaRegua_Api.Models.Users
{
    public class OpenDepositOrders
    {
        public string AccountId { get; set; }
        public string DocumentUser { get; set; }
        public string OrderId { get; set; }
        public decimal Value { get; set; }
        public PaymentType PaymentType { get; set; }
        public string CardNumber { get; set; }
    }
}
