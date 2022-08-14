using GeekShopping.PaymentAPI.Messages;
using GeekShopping.PaymentAPI.RabbitMQSender;
using GeekShopping.PaymentProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.PaymentAPI.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IProcessPayment _processPayment;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public RabbitMQPaymentConsumer(IProcessPayment processPayment, IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _processPayment = processPayment;
            _rabbitMQMessageSender = rabbitMQMessageSender;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.QueueDeclare("orderpaymentprocessqueue", false, false, false, null);
            
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                var payment = JsonSerializer.Deserialize<PaymentMessage>(content);
                ProcessPayment(payment);
                _channel.BasicAck(evt.DeliveryTag, false);
            };

            _channel.BasicConsume("orderpaymentprocessqueue", false, consumer);
            return Task.CompletedTask;
        }

        private void ProcessPayment(PaymentMessage payment)
        {
            try
            {
                var result = _processPayment.PaymentProcessor();

                var paymentResult = new UpdatePaymentResultMessage
                {
                    Status = result,
                    OrderId = payment.OrderId,
                    Email = payment.Email
                };

                // _rabbitMQMessageSender.SendMessage(paymentResult, "orderpaymentresultqueue");
                _rabbitMQMessageSender.SendMessageFanout(paymentResult);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
