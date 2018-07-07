using CourseSignUp.Domain.Model;
using System.Collections.Generic;

namespace CourseSignUp.Domain.Repositories
{
    public interface ICourseRepository
    {
        Course GetCourse(string courseCode);
        IList<Student> GetStudents(string courseCode);
        void SignUpStudent(SignUpInput input);
    }
}