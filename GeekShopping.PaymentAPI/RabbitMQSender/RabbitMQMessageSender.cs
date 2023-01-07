using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using GeekShopping.PaymentAPI.Messages;
using RabbitMQ.Client;

namespace GeekShopping.PaymentAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;
        private const string ExchangeNameFanout = "FanoutPaymentUpdateExchange";
        private const string ExchangeNameDirect = "DirectPaymentUpdateExchange";
        private const string PaymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";
        private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        public RabbitMQMessageSender(IConfiguration configuration)
        {
            _hostName = configuration["RabbitMqSettings:HostName"];
            _password = configuration["RabbitMqSettings:UserName"];
            _userName = configuration["RabbitMqSettings:Password"];
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);
                    var body = GetMessageAsByteArray(message);
                    channel.BasicPublish("", queueName, basicProperties: null, body: body);
                } 
            }
        }

        public void SendMessageFanout(BaseMessage message)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeNameFanout, ExchangeType.Fanout, durable: false);
                    var body = GetMessageAsByteArray(message);
                    channel.BasicPublish(ExchangeNameFanout, "", basicProperties: null, body: body);
                }
            }
        }

        public void SendMessageDirect(BaseMessage message)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(ExchangeNameDirect, ExchangeType.Direct, durable: false);

                    channel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null);
                    channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);
                    channel.QueueBind(PaymentEmailUpdateQueueName, ExchangeNameDirect, "PaymentEmail");
                    channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeNameDirect, "PaymentOrder");

                    var body = GetMessageAsByteArray(message);

                    channel.BasicPublish(ExchangeNameDirect, "PaymentEmail", basicProperties: null, body: body);
                    channel.BasicPublish(ExchangeNameDirect, "PaymentOrder", basicProperties: null, body: body);
                }
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, options);
            return Encoding.UTF8.GetBytes(json);
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
                return true;

            CreateConnection();

            return _connection != null;
        }
    }
}