using CourseSignUp.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}
