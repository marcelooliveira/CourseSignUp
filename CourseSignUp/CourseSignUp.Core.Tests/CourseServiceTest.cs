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

namespace CourseSignUp.Domain.Tests
{
    [TestClass]
    public class CourseServiceTest
    {
        public IServiceProvider Services { get; set; }

        IServiceCollection serviceCollection = new ServiceCollection();

        public CourseServiceTest()
        {

        }

        public CourseServiceTest(IServiceCollection serviceCollection)
        {
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
            //ApplicationContext = Services.GetService<IApplicationContext>();
        }

        [TestInitialize]
        public void Initialize()
        {
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            //Mock<IApplicationContext> mockApplicationContext = new Mock<IApplicationContext>();

            //var mockCursosSet = new List<Course>().AsQueryable();

            ////mockApplicationContext.Setup(x => x.Set<Course>())
            ////    .Returns(mockCursosSet);

            //serviceCollection.AddSingleton(typeof(IApplicationContext), mockApplicationContext.Object);

            //Mock<IStudentRepository> mockStudentRepository = new Mock<IStudentRepository>();
            //serviceCollection.AddSingleton(typeof(IStudentRepository), mockStudentRepository.Object);



            Mock<ICourseRepository> mockCourseRepository = new Mock<ICourseRepository>();
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
        public void SignUpStudent_OK()
        {
            //ICourseRepository courseRepository = Services.GetService<ICourseRepository>();

            //courseRepository.SignUpStudent(new SignUpInput(1, "José da Silva", new DateTime(1990, 1, 1)));
        }
    }
}
