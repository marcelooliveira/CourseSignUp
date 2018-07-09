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

namespace CourseSignUp.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private const string queueName = "sign-up-student";
        private const string exchangeName = "exchange-sign-up-student";
        private const string routingKey = "routing-key";

        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
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

            IConnection rabbitMQConnection = factory.CreateConnection();
            var channel = rabbitMQConnection.CreateModel();

            var json = JsonConvert.SerializeObject(input);

            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

            channel.Close();
            rabbitMQConnection.Close();

            //courseService.SignUpStudent(input);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
