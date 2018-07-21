using CourseSignUp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Repositories
{
    public interface ICourseRepository
    {
        Task<Course> GetCourse(string courseCode);
        Task<IList<Student>> GetStudents(string courseCode);
        Task SignUpStudent(SignUpInput input);
        Task UpdateCourseStats(string courseCode, int studentCount, DateTime? minBirthdate, DateTime? maxBirthdate, long birthdateTickSum);
    }

    public class CourseStatsResultDTO
    {
        public CourseStatsResultDTO(string courseCode, int studentCount, int? minAge, int? maxAge, int? avgAge)
        {
            CourseCode = courseCode;
            StudentCount = studentCount;
            MinAge = minAge;
            MaxAge = maxAge;
            AvgAge = avgAge;
        }

        public string CourseCode { get; set; }
        public int StudentCount { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? AvgAge { get; set; }
    }
}