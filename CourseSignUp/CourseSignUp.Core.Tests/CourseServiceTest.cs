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
using System.Threading.Tasks;

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
        public async Task SignUpStudent_Input_Null()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            await courseService.SignUpStudent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CourseCodeNotFoundException))]
        public async Task SignUpStudent_CourseCode_Not_Found()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            IList<Student> students = new List<Student>();

            Course course = new Course("HIS", "History", 3)
            {
                Id = 1,
                StudentCount = 0,
                MinBirthdate = (DateTime?)null,
                MaxBirthdate = (DateTime?)null,
                BirthdateTickSum = 0
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            mockCourseRepository
                .Setup(r => r.GetStudents(course.Code))
                .Returns(students);

            SignUpInput signUpInput = new SignUpInput("!@#$xpto123", "José da Silva", new DateTime(1998, 4, 3));

            await courseService.SignUpStudent(signUpInput);
        }

        [TestMethod]
        public async Task SignUpStudent_First_Student()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            IList<Student> students = new List<Student>();

            Course course = new Course("HIS", "History", 3)
            {
                Id = 1,
                StudentCount = 0,
                MinBirthdate = (DateTime?)null,
                MaxBirthdate = (DateTime?)null,
                BirthdateTickSum = 0
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            mockCourseRepository
                .Setup(r => r.GetStudents(course.Code))
                .Returns(students);

            SignUpInput signUpInput = new SignUpInput(course.Code, "José da Silva", new DateTime(1998, 4, 3));

            await courseService.SignUpStudent(signUpInput);

            mockCourseRepository.Verify(x => x.GetCourse(course.Code), Times.Once());
            mockCourseRepository.Verify(x => x.GetStudents(course.Code), Times.Once());
            mockCourseRepository.Verify(x => x.SignUpStudent(signUpInput), Times.Once());
            mockCourseRepository.Verify(x =>
                x.UpdateCourseStats(
                    signUpInput.CourseCode,
                    1,
                    signUpInput.BirthDate,
                    signUpInput.BirthDate,
                    signUpInput.BirthDate.Ticks
                )
                , Times.Once());
        }

        [TestMethod]
        public async Task SignUpStudent_One_Place_Left()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            IList<Student> students = new List<Student>
            {
                new Student("Student 1", new DateTime(1990, 2, 1)),
                new Student("Student 2", new DateTime(1995, 3, 2))
            };

            Course course = new Course("HIS", "History", 3)
            {
                Id = 1,
                StudentCount = students.Count(),
                MinBirthdate = students.Min(s => s.BirthDate),
                MaxBirthdate = students.Max(s => s.BirthDate),
                BirthdateTickSum = students.Sum(s => s.BirthDate.Ticks)
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            mockCourseRepository
                .Setup(r => r.GetStudents(course.Code))
                .Returns(students);

            SignUpInput signUpInput = new SignUpInput(course.Code, "José da Silva", new DateTime(1998, 4, 3));

            await courseService.SignUpStudent(signUpInput);

            mockCourseRepository.Verify(x => x.GetCourse(course.Code), Times.Once());
            mockCourseRepository.Verify(x => x.GetStudents(course.Code), Times.Once());
            mockCourseRepository.Verify(x => x.SignUpStudent(signUpInput), Times.Once());
            mockCourseRepository.Verify(x => 
                x.UpdateCourseStats(
                    signUpInput.CourseCode,
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
        public async Task SignUpStudent_Student_Already_Enrolled()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 30) { Id = 1 };
            IList<Student> students = new List<Student>
            {
                new Student("José da Silva", new DateTime(1990, 01, 01))
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            mockCourseRepository
                .Setup(r => r.GetStudents(course.Code))
                .Returns(students);

            SignUpInput signUpInput = new SignUpInput(course.Code, "José da Silva", new DateTime(1990, 1, 1));

            await courseService.SignUpStudent(signUpInput);

            mockCourseRepository.Verify(x => x.UpdateCourseStats(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<long>()), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(CourseOverbookException))]
        public async Task SignUpStudent_With_Full_Enrollment()
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
                .Returns(Task.FromResult(course));

            mockCourseRepository
                .Setup(r => r.GetStudents(course.Code))
                .Returns(students);

            SignUpInput signUpInput = new SignUpInput(course.Code, "José da Silva", new DateTime(1990, 1, 1));

            await courseService.SignUpStudent(signUpInput);

            mockCourseRepository.Verify(x => x.UpdateCourseStats(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<long>()), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(CourseCodeNotFoundException))]
        public async Task GetCourseStats_CourseCode_Not_Found()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 3)
            {
                Id = 1,
                StudentCount = 0,
                MinBirthdate = null,
                MaxBirthdate = null,
                BirthdateTickSum = 0
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            await courseService.GetCourseStats("!@#xpto123");
        }

        [TestMethod]
        public async Task GetCourseStats_No_Students()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 3)
            {
                Id = 1,
                StudentCount = 0,
                MinBirthdate = null,
                MaxBirthdate = null,
                BirthdateTickSum = 0
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            var courseStats = await courseService.GetCourseStats(course.Code);

            Assert.AreEqual(courseStats.CourseCode, course.Code);
            Assert.AreEqual(courseStats.StudentCount, 0);
            Assert.AreEqual(courseStats.MinAge, null);
            Assert.AreEqual(courseStats.MaxAge, null);
            Assert.AreEqual(courseStats.AvgAge, null);
        }

        [TestMethod]
        public async Task GetCourseStats_One_Student()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 3)
            {
                Id = 1,
                StudentCount = 1,
                MinBirthdate = new DateTime(1978, 1, 1),
                MaxBirthdate = new DateTime(1978, 1, 1),
                BirthdateTickSum = new DateTime(1978, 1, 1).Ticks
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            var courseStats = await courseService.GetCourseStats(course.Code);

            Assert.AreEqual(course.Code, courseStats.CourseCode);
            Assert.AreEqual(1, courseStats.StudentCount);
            Assert.AreEqual(40, courseStats.MinAge);
            Assert.AreEqual(40, courseStats.MaxAge);
            Assert.AreEqual(40, courseStats.AvgAge);
        }

        [TestMethod]
        public async Task GetCourseStats_Two_Students()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 3)
            {
                Id = 1,
                StudentCount = 2,
                MinBirthdate = new DateTime(1978, 1, 1),
                MaxBirthdate = new DateTime(1988, 1, 1),
                BirthdateTickSum =
                    new DateTime(1978, 1, 1).Ticks +
                    new DateTime(1988, 1, 1).Ticks
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            var courseStats = await courseService.GetCourseStats(course.Code);

            Assert.AreEqual(course.Code, courseStats.CourseCode);
            Assert.AreEqual(2, courseStats.StudentCount);
            Assert.AreEqual(30, courseStats.MinAge);
            Assert.AreEqual(40, courseStats.MaxAge);
            Assert.AreEqual(35, courseStats.AvgAge);
        }

        [TestMethod]
        public async Task GetCourseStats_Three_Students()
        {
            ICourseService courseService = Services.GetService<ICourseService>();

            Course course = new Course("HIS", "History", 3)
            {
                Id = 1,
                StudentCount = 3,
                MinBirthdate = new DateTime(1978, 1, 1),
                MaxBirthdate = new DateTime(1998, 1, 1),
                BirthdateTickSum =
                    new DateTime(1978, 1, 1).Ticks +
                    new DateTime(1988, 1, 1).Ticks +
                    new DateTime(1998, 1, 1).Ticks
            };

            mockCourseRepository
                .Setup(r => r.GetCourse(course.Code))
                .Returns(Task.FromResult(course));

            var courseStats = await courseService.GetCourseStats(course.Code);

            Assert.AreEqual(course.Code, courseStats.CourseCode);
            Assert.AreEqual(3, courseStats.StudentCount);
            Assert.AreEqual(20, courseStats.MinAge);
            Assert.AreEqual(40, courseStats.MaxAge);
            Assert.AreEqual(30, courseStats.AvgAge);
        }
    }
}
