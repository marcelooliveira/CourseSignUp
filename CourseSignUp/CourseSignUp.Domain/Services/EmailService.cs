using CourseSignUp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Domain.Services
{
    public class EmailService : IEmailService
    {
        public void SendSignupSuccess(Course course, SignUpInput signUpInput)
        {
            Console.WriteLine($"SUCCESS sign up email was sent to: {signUpInput.Name}");
        }

        public void SendSignupEnrollmentFull(Course course, SignUpInput signUpInput)
        {
            Console.WriteLine($"ENROLLMENT_FULL email was sent to: {signUpInput.Name}");
        }

        public void SendSignupStudentAlreadyEnrolled(Course course, SignUpInput signUpInput)
        {
            Console.WriteLine($"STUDENT_ALREADY_ENROLLED email was sent to: {signUpInput.Name}");
        }
    }
}
