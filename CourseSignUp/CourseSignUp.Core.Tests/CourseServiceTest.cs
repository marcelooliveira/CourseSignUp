using CourseSignUp.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Castle.Core.Logging;
using System;
using CourseSignUp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using CourseSignUp.Domain.Services;
using CourseSignUp.Domain.Repositories;
using CourseSignUp.Domain.Exceptions;

namespace CourseSignUp.Domain.Tests
{
    [TestClass]
    public class CourseServiceTest
    {
        private IServiceProvider Services { get; set; }
        private Mock<ICourseRepository> mockCourseRepository = new Mock<ICourseRepository>();

        IServiceCollection serviceCollection = new ServiceCollection();

        public CourseServiceTest()
        {

        }

        public CourseServiceTest(IServiceCollection serviceCollection)
        {
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
        }

        [TestInitialize]
        public void Initialize()
        {
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(ICourseRepository), mockCourseRepository.Object);
            serviceCollection.AddTransient<ICourseService, CourseService>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignUpStudent_Input_Null()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            courseService.SignUpStudent(null);
        }

        [TestMethod]
        public void SignUpStudent_One_Place_Left()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 3) { Id = 1 };
            IList<Student> students = new List<Student>
            {
                new Student("Student 1", new DateTime(1990, 2, 1)),
                new Student("Student 2", new DateTime(1995, 3, 2))
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(course);

            mockCourseRepository
                .Setup(r => r.GetStudents(course.Code))
                .Returns(students);

            SignUpInput signUpInput = new SignUpInput(course.Code, "José da Silva", new DateTime(1998, 1, 1));

            courseService.SignUpStudent(signUpInput);

            mockCourseRepository.Verify(x => x.GetCourse(course.Code), Times.Once());
            mockCourseRepository.Verify(x => x.GetStudents(course.Code), Times.Once());
            mockCourseRepository.Verify(x => x.SignUpStudent(signUpInput), Times.Once());
            mockCourseRepository.Verify(x => 
                x.UpdateCourseStats(
                    3, 
                    new DateTime(1990, 2, 1), 
                    new DateTime(1998, 4, 3), 
                        new DateTime(1990, 2, 1).Ticks + 
                        new DateTime(1995, 3, 2).Ticks +
                        new DateTime(1998, 4, 3).Ticks
                )
                , Times.Once());
        }


        [TestMethod]
        [ExpectedException(typeof(StudentAlreadyEnrolled))]
        public void SignUpStudent_Student_Already_Enrolled()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 30) { Id = 1 };
            IList<Student> students = new List<Student>
            {
                new Student("José da Silva", new DateTime(1990, 01, 01))
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(course);

            mockCourseRepository
                .Setup(r => r.GetStudents(course.Code))
                .Returns(students);

            SignUpInput signUpInput = new SignUpInput(course.Code, "José da Silva", new DateTime(1990, 1, 1));

            courseService.SignUpStudent(signUpInput);

            mockCourseRepository.Verify(x => x.UpdateCourseStats(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<long>()), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(CourseOverbookException))]
        public void SignUpStudent_With_Full_Enrollment()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 5) { Id = 1 };
            IList<Student> students = new List<Student>
            {
                new Student("Student 1", new DateTime(1990, 01, 01)),
                new Student("Student 2", new DateTime(1990, 01, 01)),
                new Student("Student 3", new DateTime(1990, 01, 01)),
                new Student("Student 4", new DateTime(1990, 01, 01)),
                new Student("Student 5", new DateTime(1990, 01, 01))
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(course);

            mockCourseRepository
                .Setup(r => r.GetStudents(course.Code))
                .Returns(students);

            SignUpInput signUpInput = new SignUpInput(course.Code, "José da Silva", new DateTime(1990, 1, 1));

            courseService.SignUpStudent(signUpInput);

            mockCourseRepository.Verify(x => x.UpdateCourseStats(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<long>()), Times.Never());
        }
    }
}
