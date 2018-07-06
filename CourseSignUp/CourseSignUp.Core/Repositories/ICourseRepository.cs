using CourseSignUp.Core.Model;

namespace CourseSignUp.Core.Repositories
{
    public interface ICourseRepository
    {
        void SignUpStudent(SignUpInput input);
    }
}