using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseSignUp.Domain.Model;
using CourseSignUp.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using CourseSignUp.Domain.Repositories;
using CourseSignUp.Domain.Services;
using Microsoft.Extensions.Logging;

namespace CourseSignUp.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IMessageQueueClientService messageQueueClientService;
        private readonly ILogger<CourseController> logger;

        public CourseController(ILogger<CourseController> logger, ICourseService courseService, IMessageQueueClientService messageQueueClientService)
        {
            this.logger = logger;
            this.courseService = courseService;
            this.messageQueueClientService = messageQueueClientService;
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
            messageQueueClientService.EnqueueSignUpMessage(input);
        }
    }
}
