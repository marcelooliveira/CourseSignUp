using CourseSignUp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CourseSignUp.Domain.Repositories;

namespace CourseSignUp.Data.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly IStudentRepository studentRepository;

        //public CourseRepository()
        //{

        //}

        public CourseRepository(IApplicationContext context,
            IStudentRepository studentRepository) : base(context)
        {
            base.context = context;
            this.studentRepository = studentRepository;
        }

        public void SignUpStudent(SignUpInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            Microsoft.EntityFrameworkCore.DbSet<Course> set = context.Set<Course>();
            Course course = set.Where(c => c.Id == input.CourseId).SingleOrDefault();

            if (course != null)
            {
                throw new ArgumentException("Course Id not found.");
            }

            var student = studentRepository.Save(input.Name, input.BirthDate);
            context.Set<Enrollment>().Add(new Enrollment(course, student));

            context.SaveChanges();
        }
    }
}
