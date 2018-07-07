using CourseSignUp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using CourseSignUp.Domain.Model;

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

            context.Courses.Add(new Course("HIS", "History", 5));
            context.Courses.Add(new Course("ENG", "English", 5));
            context.Courses.Add(new Course("PHI", "Philosophy", 5));
        }
    }
}
