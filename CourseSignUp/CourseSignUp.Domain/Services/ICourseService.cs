using CourseSignUp.Domain.Model;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services
{
    public interface ICourseService
    {
        Task SignUpStudent(SignUpInput input);
    }
}
