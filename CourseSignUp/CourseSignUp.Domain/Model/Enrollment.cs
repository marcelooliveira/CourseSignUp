using System;
using System.ComponentModel.DataAnnotations;

namespace CourseSignUp.Domain.Model
{
    public class Enrollment : BaseModel
    {
        public Enrollment()
        {

        }
        public Enrollment(int courseId, int studentId)
        {
            CourseId = courseId;
            StudentId = studentId;
        }

        public Enrollment(Course course, Student student)
        {
            Course = course;
            Student = student;
        }

        [Required]
        public virtual int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public virtual int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
