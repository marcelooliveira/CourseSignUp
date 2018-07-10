using CourseSignUp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CourseSignUp.Domain.Model;
using System.Linq;
using CourseSignUp.Data;

namespace CourseSignUp
{
    class DataService : IDataService
    {
        private readonly ApplicationContext context;

        public DataService(ApplicationContext contexto)
        {
            this.context = contexto;
        }

        public void InitializeDB()
        {
            context.Database.Migrate();

            AddInitialCourses();
        }

        private void AddInitialCourses()
        {
            if (context.Courses.Count() == 0)
            {
                Teacher teacher1 = new Teacher("John Doe");
                Teacher teacher2 = new Teacher("José da Silva");
                Teacher teacher3 = new Teacher("Fulano de Tal");
                context.Teachers.Add(teacher1);
                context.Teachers.Add(teacher2);
                context.Teachers.Add(teacher3);
                context.SaveChanges();

                context.Courses.Add(new Course("HIS", "History", 5, teacher1.Id));
                context.Courses.Add(new Course("ENG", "English", 5, teacher2.Id));
                context.Courses.Add(new Course("PHI", "Philosophy", 5, teacher3.Id));
                context.SaveChanges();
            }
        }
    }
}
