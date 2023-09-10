using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Enums;
using NaRegua_Api.Common.Statics;
using NaRegua_Api.QueueService;
using NaRegua_Api.RedisService;
using Newtonsoft.Json;

namespace NaRegua_Api.Providers.Fakes
{
    public class OrderProviderFake : IOrderProvider
    {
        private readonly IQueueService _queueService;
        private readonly IRedisService _redisService;
        public OrderProviderFake(IQueueService queueService, IRedisService redisService) 
        { 
            _queueService = queueService;
            _redisService = redisService;
        }
        public Task<OrderStatus> GetPaymentOrderStatus(string orderId)
        {
            var order = OrderPaymentStatus.OrdersStatus[orderId];
            if (order != OrderStatus.PendingPayment)
            {
                return Task.FromResult(order);
            }
            else
            {
                var value = _redisService.GetString(orderId);

                if (value != "0")
                {
                    OrderPaymentStatus.OrdersStatus[orderId] = (OrderStatus) int.Parse(value);
                }
            }
            
            return Task.FromResult(OrderPaymentStatus.OrdersStatus[orderId]);
        }
        public void SetPaymentOrder(string orderId, PaymentType type, string cardNumber)
        {
            var senderPaymentOrder = new
            {
                OrderId = orderId,
                PaymentType = type,
                CardNumber = cardNumber
            };

            var messageJson = JsonConvert.SerializeObject(senderPaymentOrder);
            _queueService.PublishMessage(messageJson);

            OrderPaymentStatus.OrdersStatus.Add(orderId, OrderStatus.PendingPayment);
        }
    }
}
