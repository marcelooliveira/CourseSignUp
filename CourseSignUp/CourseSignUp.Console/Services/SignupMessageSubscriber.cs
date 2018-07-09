using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using CourseSignUp.Domain.Model;
using CourseSignUp.Domain.Services;

namespace BackgroundTasksSample.Services
{
    public class SignupMessageSubscriber
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;
        private readonly IModel _channel;
        private readonly IConnection _rabbitMQConn;
        private readonly ICourseService _courseService;

        private const string queueName = "sign-up-student";
        private const string exchangeName = "exchange-sign-up-student";
        private const string routingKey = "routing-key";

        public SignupMessageSubscriber(IBackgroundTaskQueue taskQueue, ILogger<MonitorLoop> logger, IApplicationLifetime applicationLifetime, IConnection rabbitMQConn, ICourseService courseService)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
            _rabbitMQConn = rabbitMQConn;
            _courseService = courseService;
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

            //var consumer = new EventingBasicConsumer(channel);
            //consumer.Received += (ch, ea) =>
            //{
            //    _logger.LogInformation($"consumer.Received");

            //    var body = ea.Body;
            //    // ... process the message
            //    channel.BasicAck(ea.DeliveryTag, false);
            //};

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);

            while (!_cancellationToken.IsCancellationRequested)
            {
                bool noAck = false;
                BasicGetResult result = channel.BasicGet(queueName, noAck);
                if (result == null)
                {
                    // No message available at this time.
                }
                else
                {
                    IBasicProperties props = result.BasicProperties;
                    byte[] body = result.Body;
                    var json = System.Text.Encoding.UTF8.GetString(body);
                    _logger.LogInformation($"*** MESSAGE RECEIVED ***: " + json);
                    //...
                    // acknowledge receipt of the message
                    channel.BasicAck(result.DeliveryTag, false);

                    SignUpInput signUpInput = null;
                    try
                    {
                        signUpInput = JsonConvert.DeserializeObject<SignUpInput>(json);
                        // Enqueue a background work item
                        ProcessInput(signUpInput);
                    }
                    catch (Exception)
                    {
                    }

                }
            }

            channel.Close();
            _rabbitMQConn.Close();
        }

        private void ProcessInput(SignUpInput signUpInput)
        {
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                var guid = Guid.NewGuid().ToString();

                _logger.LogInformation(
                        $"RUNNING: Queued Background Task - CourseCode: {signUpInput.CourseCode}, Name: {signUpInput.Name}, BirthDate: {signUpInput.BirthDate}");

                await _courseService.SignUpStudent(signUpInput);

                //await Task.Delay(TimeSpan.FromSeconds(5), token);

                _logger.LogInformation(
                $"COMPLETED: Queued Background Task - CourseCode: {signUpInput.CourseCode}, Name: {signUpInput.Name}, BirthDate: {signUpInput.BirthDate}");
            });
        }
    }
}