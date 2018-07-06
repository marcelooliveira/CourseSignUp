﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseSignUp.Domain.Model;
using CourseSignUp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using CourseSignUp.Domain.Repositories;

namespace CourseSignUp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ICourseRepository courseRepository;

        public ValuesController(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
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
            courseRepository.SignUpStudent(input);
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
