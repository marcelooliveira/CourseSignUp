using CourseSignUp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Data.Repositories
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        protected IApplicationContext context;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(IApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
    }
}
