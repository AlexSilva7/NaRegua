using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using RabbitMQ.Client;
using System.Text;

namespace NaRegua_Api.QueueService
{
    public class RabbitMQService : IQueueService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            var factory = new ConnectionFactory
            {
                HostName = AppSettings.QueueConfig.Host,
                Port = AppSettings.QueueConfig.Port,
                UserName = AppSettings.QueueConfig.User,
                Password = AppSettings.QueueConfig.Pass
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclarePassive("QueueOrder");
                Console.WriteLine($"A fila QueueOrder já existe.");
            }
            catch (Exception)
            {
                //_channel.QueueDeclare("QueueOrder", durable: true, exclusive: false, autoDelete: false);
            }
        }
        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: "QueueOrder", basicProperties: null, body: body);
        }
        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }

        public QueueInfo GetConnectionInfo()
        {
            try
            {
                var countMessage = _channel.MessageCount("QueueOrder");

                return new QueueInfo
                {
                    QueueProvider = "RabbitMQ",
                    IsConnected = _connection.IsOpen,
                    QueueLength = (int)countMessage,
                    QueueLevel = 0
                };
            }
            catch(Exception ex)
            {
                return new QueueInfo
                {
                    QueueProvider = "RabbitMQ",
                    IsConnected = false,
                    QueueLength = 0
                };
            }
        }
    }
}