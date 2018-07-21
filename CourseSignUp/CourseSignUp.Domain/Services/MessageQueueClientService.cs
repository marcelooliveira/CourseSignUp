using System;
using CourseSignUp.Domain.Model;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace CourseSignUp.Domain.Services
{
    public class MessageQueueClientService : IMessageQueueClientService
    {
        private const string queueName = "sign-up-student";
        private const string exchangeName = "exchange-sign-up-student";
        private const string routingKey = "routing-key";
        private const string RABBITMQ_USERNAME = "guest";
        private const string RABBITMQ_PASSWORD = "guest";
        private const string RABBITMQ_VIRTUAL_HOST = "/";
        private const string RABBITMQ_HOSTNAME = "localhost";

        private readonly ILogger<MessageQueueClientService> logger;
        public MessageQueueClientService(ILogger<MessageQueueClientService> logger)
        {
            this.logger = logger;
        }

        public void EnqueueSignUpMessage(SignUpInput input)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = RABBITMQ_USERNAME,
                Password = RABBITMQ_PASSWORD,
                VirtualHost = RABBITMQ_VIRTUAL_HOST,
                HostName = RABBITMQ_HOSTNAME
            };

            try
            {
                IConnection rabbitMQConnection = factory.CreateConnection();
                var channel = rabbitMQConnection.CreateModel();

                var json = JsonConvert.SerializeObject(input);

                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

                channel.Close();
                rabbitMQConnection.Close();
            }
            catch (Exception exc)
            {
                logger.LogError(exc.ToString());
                throw;
            }
        }
    }
}
