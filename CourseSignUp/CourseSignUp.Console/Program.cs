using BackgroundTasksSample.Services;
using CourseSignUp.Data;
using CourseSignUp.Data.Repositories;
using CourseSignUp.Domain.Repositories;
using CourseSignUp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace CourseSignUp.Console
{
    class Program
    {
        private const string queueName = "sign-up-student";
        private const string exchangeName = "exchange-sign-up-student";
        private const string routingKey = "routing-key";

        public static IConfigurationRoot Configuration { get; set; }

        static async Task Main(string[] args)
        {
            //var builder = new ConfigurationBuilder()
            //    //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    // You can't use environment specific configuration files like this
            //    // becuase IHostingEnvironment is an ASP.NET Core specific interface
            //    //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //    //.AddUserSecrets()
            //    .AddEnvironmentVariables();

            //Configuration = builder.Build();

            var host = new HostBuilder()
                .ConfigureLogging((hostContext, config) =>
                {
                    config.AddConsole();
                    config.AddDebug();
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    //config.AddEnvironmentVariables();
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddSingleton<MonitorLoop>();
                    services.AddSingleton<SignupMessageSubscriber>();

                    #region snippet1
                    services.AddHostedService<TimedHostedService>();
                    #endregion

                    #region snippet2
                    services.AddHostedService<ConsumeScopedServiceHostedService>();
                    services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
                    #endregion

                    #region snippet3
                    services.AddHostedService<QueuedHostedService>();
                    services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
                    #endregion

                    var configuration = hostContext.Configuration.ToString();
                    var children = hostContext.Configuration.GetChildren().ToList();

                    services.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("Default")));

                    services.AddTransient<IApplicationContext, ApplicationContext>();
                    services.AddTransient<ICourseService, CourseService>();
                    services.AddTransient<ICourseRepository, CourseRepository>();
                    services.AddTransient<IStudentRepository, StudentRepository>();

                    ConnectionFactory factory = new ConnectionFactory
                    {
                        UserName = "guest",
                        Password = "guest",
                        VirtualHost = "/",
                        HostName = "localhost"
                    };

                    IConnection conn = factory.CreateConnection();
                    services.AddSingleton(typeof(IConnection), conn);
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                // Start the host
                await host.StartAsync();

                // Monitor for new background queue work items
                //var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
                //monitorLoop.StartMonitorLoop();

                //ConnectionFactory factory = new ConnectionFactory
                //{
                //    UserName = "guest",
                //    Password = "guest",
                //    VirtualHost = "/",
                //    HostName = "localhost"
                //};

                //IConnection rabbitMQConnection = factory.CreateConnection();

                var signupMessageSubscriber = host.Services.GetRequiredService<SignupMessageSubscriber>();
                signupMessageSubscriber.StartSignupMessageSubscriber();

                //var channel = rabbitMQConnection.CreateModel();

                //var consumer = new EventingBasicConsumer(channel);
                //consumer.Received += (ch, ea) =>
                //{
                //    //_logger.LogInformation($"consumer.Received");

                //    var body = ea.Body;
                //    // ... process the message
                //    channel.BasicAck(ea.DeliveryTag, false);
                //};

                //channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                //channel.QueueDeclare(queueName, false, false, false, null);
                //channel.QueueBind(queueName, exchangeName, routingKey, null);

                //var response = channel.QueueDeclarePassive(queueName);
                //String consumerTag = channel.BasicConsume(queueName, false, consumer);

                // Wait for the host to shutdown
                await host.WaitForShutdownAsync();

                //channel.Close();
                //rabbitMQConnection.Close();
            }
        }
    }
}
