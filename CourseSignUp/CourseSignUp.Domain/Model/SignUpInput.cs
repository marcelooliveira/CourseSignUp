using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Domain.Model
{
    public class SignUpInput
    {
        public SignUpInput(string courseCode, string name, DateTime birthDate)
        {
            CourseCode = courseCode;
            Name = name;
            BirthDate = birthDate;
        }

        public string CourseCode { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
