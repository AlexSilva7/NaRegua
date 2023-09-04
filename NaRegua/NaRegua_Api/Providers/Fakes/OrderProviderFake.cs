using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Enums;
using NaRegua_Api.QueueService;
using NaRegua_Api.RedisService;
using Newtonsoft.Json;
using static Google.Rpc.Context.AttributeContext.Types;

namespace NaRegua_Api.Providers.Fakes
{
    public class OrderProviderFake : IOrderProvider
    {
        private readonly IQueueService _queueService;
        private readonly IRedisService _redisService;

        public static Dictionary<string, OrderStatus> _orderPaymentStatus = new Dictionary<string, OrderStatus>();
        public OrderProviderFake(IQueueService queueService) 
        { 
            _queueService = queueService;
        }
        public Task<OrderStatus> GetPaymentOrderStatus(string orderId)
        {
            throw new NotImplementedException();
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

            _orderPaymentStatus.Add(orderId, OrderStatus.PendingPayment);
        }
    }
}
