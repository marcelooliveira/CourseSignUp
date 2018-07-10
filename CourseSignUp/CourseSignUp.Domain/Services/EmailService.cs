using CourseSignUp.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger logger;
        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }

        public void SendSignupSuccess(Course course, SignUpInput signUpInput)
        {
            logger.LogInformation($"SUCCESS sign up email was sent to: {signUpInput.Name}");
        }

        public void SendSignupEnrollmentFull(Course course, SignUpInput signUpInput)
        {
            logger.LogInformation($"ENROLLMENT_FULL email was sent to: {signUpInput.Name}");
        }

        public void SendSignupStudentAlreadyEnrolled(Course course, SignUpInput signUpInput)
        {
            logger.LogInformation($"STUDENT_ALREADY_ENROLLED email was sent to: {signUpInput.Name}");
        }
    }
}
