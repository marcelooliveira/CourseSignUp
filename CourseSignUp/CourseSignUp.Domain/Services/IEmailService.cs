using CourseSignUp.Domain.Model;

namespace CourseSignUp.Domain.Services
{
    public interface IEmailService
    {
        void SendSignupSuccess(Course course, SignUpInput signUpInput);
        void SendSignupStudentAlreadyEnrolled(Course course, SignUpInput signUpInput);
        void SendSignupEnrollmentFull(Course course, SignUpInput signUpInput);
    }
}