using CourseSignUp.Domain.Model;

namespace CourseSignUp.Domain.Repositories
{
    public interface ICourseRepository
    {
        void SignUpStudent(SignUpInput input);
    }
}