using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseSignUp.Domain.Model;
using CourseSignUp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using CourseSignUp.Domain.Repositories;
using CourseSignUp.Domain.Services;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace CourseSignUp.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private const string queueName = "sign-up-student";
        private const string exchangeName = "exchange-sign-up-student";
        private const string routingKey = "routing-key";

        private readonly ICourseService courseService;
        private readonly ILogger<CourseController> logger;

        public CourseController(ILogger<CourseController> logger, ICourseService courseService)
        { 
            this.logger = logger;
            this.courseService = courseService;
        }

        // GET api/Course/HIS
        [HttpGet("{courseCode}")]
        public async Task<CourseStatsResultDTO> Get(string courseCode)
        {
            try
            {
                return await courseService.GetCourseStats(courseCode);
            }
            catch (Exception exc)
            {
                logger.LogError(exc.ToString());
                throw;
            }
        }

        // POST api/Course
        [HttpPost]
        public void Post([FromBody]SignUpInput input)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost"
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
