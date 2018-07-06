using CourseSignUp.Domain.Model;

namespace CourseSignUp.Domain.Services
{
    public interface ICourseService
    {
        void SignUpStudent(SignUpInput input);
    }
}
