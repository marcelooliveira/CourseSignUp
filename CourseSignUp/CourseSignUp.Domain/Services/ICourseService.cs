using CourseSignUp.Domain.Model;
using CourseSignUp.Domain.Repositories;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services
{
    public interface ICourseService
    {
        Task SignUpStudent(SignUpInput input);
        Task<CourseStatsResultDTO> GetCourseStats(string courseCode);
    }
}
