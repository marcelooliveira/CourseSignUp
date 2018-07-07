using CourseSignUp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CourseSignUp.Domain.Repositories;

namespace CourseSignUp.Data.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly IStudentRepository studentRepository;

        //public CourseRepository()
        //{

        //}

        public CourseRepository(IApplicationContext context,
            IStudentRepository studentRepository) : base(context)
        {
            base.context = context;
            this.studentRepository = studentRepository;
        }

        public Course GetCourse(string courseCode)
        {
            Course course = dbSet.Where(c => c.Code == courseCode).SingleOrDefault();
            if (course == null)
            {
                throw new ArgumentException($"Course not found: {courseCode}");
            }
            return course;
        }

        public IList<Student> GetStudents(string courseCode)
        {
            return context.Set<Enrollment>()
                .Where(e => e.Course.Code == courseCode)
                .Select(e => e.Student)
                .ToList();
        }

        public void SignUpStudent(SignUpInput input)
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

            var student = studentRepository.Save(input.Name, input.BirthDate);
            context.SaveChanges();

            context.Set<Enrollment>().Add(new Enrollment(course.Id, student.Id));

            context.SaveChanges();
        }

    }
}
