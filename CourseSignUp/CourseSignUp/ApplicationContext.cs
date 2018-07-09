//using CourseSignUp.Data;
//using CourseSignUp.Domain.Model;
//using JetBrains.Annotations;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CourseSignUp
//{
//    public class ApplicationContext : DbContext, IApplicationContext
//    {
//        public DbSet<Course> Courses { get; set; }
//        public DbSet<Student> Students { get; set; }
//        public DbSet<Teacher> Teachers { get; set; }
//        public DbSet<Enrollment> Enrollments { get; set; }

//        public ApplicationContext(DbContextOptions options) : base(options)
//        {
//        }
//    }
//}
