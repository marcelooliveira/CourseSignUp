using System;

namespace CourseSignUp.Domain.Model
{
    public class Student : BaseModel
    {
        public Student(string name, DateTime birthDate)
        {
            Name = name;
            BirthDate = birthDate;
        }

        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
