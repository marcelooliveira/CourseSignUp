using CourseSignUp.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Domain.Services
{
    public class BaseService
    {
        protected ILogger logger;
        public BaseService(ILogger<BaseService> logger)
        {
            this.logger = logger;
        }
    }
}
