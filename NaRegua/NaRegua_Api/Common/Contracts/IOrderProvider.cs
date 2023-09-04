using NaRegua_Api.Common.Enums;

namespace NaRegua_Api.Common.Contracts
{
    public interface IOrderProvider
    {
        Task<OrderStatus> GetPaymentOrderStatus(string orderId);
        void SetPaymentOrder(string orderId, PaymentType type, string cardNumber);
    }
}
