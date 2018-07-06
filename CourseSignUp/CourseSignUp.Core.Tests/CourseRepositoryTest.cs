using CourseSignUp.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Castle.Core.Logging;
using System;

namespace CourseSignUp.Core.Tests
{
    [TestClass]
    public class CourseRepositoryTest
    {
        public IServiceProvider Services { get; set; }
        //public IApplicationContext ApplicationContext { get; set; }

        IServiceCollection serviceCollection = new ServiceCollection();

        public CourseRepositoryTest(IServiceCollection serviceCollection)
        {
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();
            //ApplicationContext = Services.GetService<IApplicationContext>();
        }

        [TestInitialize]
        public void Initialize()
        {
            ConfigureServices(serviceCollection);
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICourseRepository, CourseRepository>();
        }

        [TestMethod]
        public void SignUpStudent_Input_Null()
        {
            Mock<IApplicationContext> applicationContext = new Mock<IApplicationContext>();
            serviceCollection.AddSingleton(typeof(IApplicationContext), applicationContext);
            ICourseRepository courseRepository = Services.GetService<ICourseRepository>();

        }
    }
}
