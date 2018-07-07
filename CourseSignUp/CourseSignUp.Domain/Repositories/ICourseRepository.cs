using CourseSignUp.Domain.Model;
using System.Collections.Generic;

namespace CourseSignUp.Domain.Repositories
{
    public interface ICourseRepository
    {
        Course GetCourse(int courseId);
        IList<Student> GetStudents(int courseId);
        void SignUpStudent(SignUpInput input);
    }
}