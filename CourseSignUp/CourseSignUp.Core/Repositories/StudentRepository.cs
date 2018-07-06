using CourseSignUp.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Core.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IApplicationContext context) : base(context)
        {
            base.context = context;
        }

        public Student Save(string name, DateTime birthDate)
        {
            var student = dbSet.Add(new Student(name, birthDate));
            context.SaveChanges();
            return student.Entity;
        }
    }
}
