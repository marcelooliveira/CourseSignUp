﻿using CourseSignUp.Domain.Exceptions;
using CourseSignUp.Domain.Model;
using CourseSignUp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseSignUp.Domain.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public async Task SignUpStudent(SignUpInput input)
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

            await courseRepository.SignUpStudent(input);

            DateTime minBirthdate = course.MinBirthdate ?? DateTime.MaxValue;
            if (DateTime.Compare(minBirthdate, input.BirthDate) < 0)
                minBirthdate = input.BirthDate;

            DateTime maxBirthdate = course.MaxBirthdate ?? DateTime.MinValue;
            if (DateTime.Compare(minBirthdate, input.BirthDate) > 0)
                maxBirthdate = input.BirthDate;

            long birthdateTickSum = course.BirthdateTickSum + input.BirthDate.Ticks;

            await courseRepository.UpdateCourseStats(students.Count(), minBirthdate, maxBirthdate, birthdateTickSum);
        }
    }
}
