using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace BackgroundTasksSample.Services
{
    public class SignupMessageSubscriber
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;
        private readonly IModel _channel;
        private readonly IConnection _rabbitMQConn;

        private const string queueName = "sign-up-student";
        private const string exchangeName = "exchange-sign-up-student";
        private const string routingKey = "routing-key";

        public SignupMessageSubscriber(IBackgroundTaskQueue taskQueue, ILogger<MonitorLoop> logger, IApplicationLifetime applicationLifetime, IConnection rabbitMQConn)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
            _rabbitMQConn = rabbitMQConn;
        }

        public void StartSignupMessageSubscriber()
        {
            // Run a console user input loop in a background thread
            Task.Run(() => ListenToMessages());
        }

        public void ListenToMessages()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost"
            };

            IConnection rabbitMQConnection = factory.CreateConnection();
            var channel = rabbitMQConnection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                _logger.LogInformation($"consumer.Received");

                var body = ea.Body;
                // ... process the message
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            while (!_cancellationToken.IsCancellationRequested)
            {
                // Enqueue a background work item
                _taskQueue.QueueBackgroundWorkItem(async token =>
                {
                    var guid = Guid.NewGuid().ToString();

                    for (int delayLoop = 0; delayLoop < 3; delayLoop++)
                    {
                        _logger.LogInformation(
                            $"Queued Background Task {guid} is running. {delayLoop}/3");
                        await Task.Delay(TimeSpan.FromSeconds(5), token);
                    }

                    _logger.LogInformation(
                        $"Queued Background Task {guid} is complete. 3/3");
                });
            }

            channel.Close();
            _rabbitMQConn.Close();
        }
    }
}