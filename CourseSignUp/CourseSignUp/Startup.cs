using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseSignUp.Data;
using CourseSignUp.Data.Repositories;
using CourseSignUp.Domain.Repositories;
using CourseSignUp.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CourseSignUp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            string connectionString = Configuration.GetConnectionString("Default");
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddTransient<IApplicationContext, ApplicationContext>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IDataService, DataService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            serviceProvider.GetService<IDataService>().InitializeDB();
        }
    }
}
