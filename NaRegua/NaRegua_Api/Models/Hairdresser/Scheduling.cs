using NaRegua_Api.Common.Enums;

namespace NaRegua_Api.Models.Hairdresser
{
    public class Scheduling
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime DateTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
