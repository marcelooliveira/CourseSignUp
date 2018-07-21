using CourseSignUp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CourseSignUp.Domain.Repositories;
using System.Threading.Tasks;

namespace CourseSignUp.Data.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly IStudentRepository studentRepository;

        public CourseRepository(IApplicationContext context,
            IStudentRepository studentRepository) : base(context)
        {
            base.context = context;
            this.studentRepository = studentRepository;
        }

        public Task<Course> GetCourse(string courseCode)
        {
            return Task.Run(() =>
            {
                Course course = dbSet.Where(c => c.Code == courseCode).SingleOrDefault();
                if (course == null)
                {
                    throw new ArgumentException($"Course not found: {courseCode}");
                }
                return course;
            });
        }

        public IList<Student> GetStudents(string courseCode)
        {
            return context.Set<Enrollment>()
                .Where(e => e.Course.Code == courseCode)
                .Select(e => e.Student)
                .ToList();
        }

        public async Task SignUpStudent(SignUpInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            Course course = dbSet.Where(c => c.Code == input.CourseCode).SingleOrDefault();

            if (course == null)
            {
                throw new ArgumentException("Course code not found.");
            }

            var student = new Student(input.Name, input.BirthDate);

            context.Set<Enrollment>().Add(new Enrollment(course, student));

            await context.SaveChangesAsync();
        }

        public Task UpdateCourseStats(string courseCode, int studentCount, DateTime? minBirthdate, DateTime? maxBirthdate, long birthdateTickSum)
        {
            return Task.Run(() =>
            {
                var course = dbSet.Where(c => c.Code == courseCode).SingleOrDefault();

                if (course == null)
                {
                    throw new ArgumentException("Course code not found.");
                }

                course.StudentCount = studentCount;
                course.MinBirthdate = minBirthdate;
                course.MaxBirthdate = maxBirthdate;
                course.BirthdateTickSum = birthdateTickSum;
                context.SaveChanges();
            });
        }
    }
}
