using CourseSignUp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CourseSignUp.Domain.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Course> GetCourse(string courseCode)
        {
            return await dbSet.Where(c => c.Code == courseCode).SingleOrDefaultAsync();
        }

        public async Task<IList<Student>> GetStudents(string courseCode)
        {
            return await context.Set<Enrollment>()
                .Where(e => e.Course.Code == courseCode)
                .Select(e => e.Student)
                .ToListAsync();
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

        public async Task UpdateCourseStats(string courseCode, int studentCount, DateTime? minBirthdate, DateTime? maxBirthdate, long birthdateTickSum)
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
            await context.SaveChangesAsync();
        }
    }
}
