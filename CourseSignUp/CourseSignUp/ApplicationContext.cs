using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseSignUp
{
    public class ApplicationContext : DbContext
    {
        //DbSet<>

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
