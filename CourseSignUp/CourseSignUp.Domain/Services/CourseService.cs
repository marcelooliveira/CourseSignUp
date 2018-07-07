using CourseSignUp.Domain.Exceptions;
using CourseSignUp.Domain.Model;
using CourseSignUp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Domain.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public void SignUpStudent(SignUpInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var course = courseRepository.GetCourse(input.CourseCode);
            var students = courseRepository.GetStudents(input.CourseCode);

            if (students.Contains(new Student(input.Name, input.BirthDate)))
            {
                throw new StudentAlreadyEnrolled();
            }

            if (students.Count >= course.MaxStudentCount)
            {
                throw new CourseOverbookException();
            }

            courseRepository.SignUpStudent(input);
        }
    }
}
