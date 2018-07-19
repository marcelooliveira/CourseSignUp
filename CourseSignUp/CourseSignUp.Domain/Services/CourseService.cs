using CourseSignUp.Domain.Exceptions;
using CourseSignUp.Domain.Model;
using CourseSignUp.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services
{
    public class CourseService : BaseService, ICourseService
    {
        private readonly ICourseRepository courseRepository;
        private readonly IEmailService emailService;

        public CourseService(ILogger<CourseService> logger, ICourseRepository courseRepository, IEmailService emailService) : base(logger)
        {
            this.logger = logger;
            this.courseRepository = courseRepository;
            this.emailService = emailService;
        }

        public async Task<CourseStatsResultDTO> GetCourseStats(string courseCode)
        {
            var course = await courseRepository.GetCourse(courseCode);
            if (course == null)
            {
                throw new CourseCodeNotFoundException();
            }

            DateTime? avgBirthdate = GetAvgBirthdate(course);

            int? minAge = default;
            int? maxAge = default;
            int? avgAge = default;

            if (course.StudentCount > 0)
            {
                minAge = GetAge(course.MaxBirthdate.Value);
                maxAge = GetAge(course.MinBirthdate.Value);
                long avgTicks = (long)((double)course.BirthdateTickSum / (double)course.StudentCount);
                avgAge = GetAge(new DateTime(avgTicks));
            }

            return new CourseStatsResultDTO(courseCode, course.StudentCount, minAge, maxAge, avgAge);
        }

        private static DateTime? GetAvgBirthdate(Course course)
        {
            if (course.StudentCount == 0)
            {
                return (DateTime?)null;
            }
            if (course.BirthdateTickSum == 0)
            {
                return (DateTime?)null;
            }
            return new DateTime((long)((double)course.BirthdateTickSum / (double)course.StudentCount));
        }

        public async Task SignUpStudent(SignUpInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var course = await courseRepository.GetCourse(input.CourseCode);

            if (course == null)
            {
                throw new CourseCodeNotFoundException();
            }

            var students = courseRepository.GetStudents(input.CourseCode);

            if (students.Contains(new Student(input.Name, input.BirthDate)))
            {
                emailService.SendSignupStudentAlreadyEnrolled(course, input);
                return;
            }

            if (students.Count >= course.MaxStudentCount)
            {
                emailService.SendSignupEnrollmentFull(course, input);
                return;
            }

            await courseRepository.SignUpStudent(input);

            int studentCount = course.StudentCount + 1;

            DateTime minBirthdate = course.MinBirthdate ?? DateTime.MaxValue;
            if (DateTime.Compare(input.BirthDate, minBirthdate) < 0)
                minBirthdate = input.BirthDate;

            DateTime maxBirthdate = course.MaxBirthdate ?? DateTime.MinValue;
            if (DateTime.Compare(maxBirthdate, input.BirthDate) < 0)
                maxBirthdate = input.BirthDate;

            long birthdateTickSum = course.BirthdateTickSum + input.BirthDate.Ticks;

            await courseRepository.UpdateCourseStats(input.CourseCode, studentCount, minBirthdate, maxBirthdate, birthdateTickSum);

            emailService.SendSignupSuccess(course, input);
        }

        int GetAge(DateTime birthDate)
        {
            DateTime n = DateTime.Now;
            int age = n.Year - birthDate.Year;

            if (n.Month < birthDate.Month || (n.Month == birthDate.Month && n.Day < birthDate.Day))
                age--;

            return age;
        }
    }
}
