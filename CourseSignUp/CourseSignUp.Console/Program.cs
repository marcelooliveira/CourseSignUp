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
            var host = new HostBuilder()
                .ConfigureLogging((hostContext, config) =>
                {
                    config.AddConsole();
                    config.AddDebug();
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddSingleton<SignupMessageSubscriber>();

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

                var signupMessageSubscriber = host.Services.GetRequiredService<SignupMessageSubscriber>();
                signupMessageSubscriber.StartSignupMessageSubscriber();

                // Wait for the host to shutdown
                await host.WaitForShutdownAsync();
            }
        }
    }
}
