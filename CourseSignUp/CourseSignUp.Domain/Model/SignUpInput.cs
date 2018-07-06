using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Domain.Model
{
    public class SignUpInput
    {
        public SignUpInput(int courseId, string name, DateTime birthDate)
        {
            CourseId = courseId;
            Name = name;
            BirthDate = birthDate;
        }

        public int CourseId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
