using CourseSignUp.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Repositories
{
    public interface ICourseRepository
    {
        Course GetCourse(string courseCode);
        IList<Student> GetStudents(string courseCode);
        Task SignUpStudent(SignUpInput input);
    }
}