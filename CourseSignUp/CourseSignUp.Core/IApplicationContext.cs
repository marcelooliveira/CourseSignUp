using CourseSignUp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CourseSignUp.Data
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
