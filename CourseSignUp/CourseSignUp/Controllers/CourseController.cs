﻿using System;
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

        // GET api/Course
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/Course/HIS
        [HttpGet("{courseCode}")]
        public async Task<CourseStatsResultDTO> Get(string courseCode)
        {
            return await courseService.GetCourseStats(courseCode);
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

            IConnection rabbitMQConnection = factory.CreateConnection();
            var channel = rabbitMQConnection.CreateModel();

            var json = JsonConvert.SerializeObject(input);

            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

            channel.Close();
            rabbitMQConnection.Close();
        }

        // PUT api/Course/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/Course/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
