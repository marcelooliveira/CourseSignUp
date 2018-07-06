using CourseSignUp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CourseSignUp
{
    public interface IApplicationContext
    {
        DbSet<Course> Courses { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Teacher> Teachers { get; set; }

        DbSet<T> Set<T>() where T: class;
        DatabaseFacade Database { get; }

        int SaveChanges();
    }
}
