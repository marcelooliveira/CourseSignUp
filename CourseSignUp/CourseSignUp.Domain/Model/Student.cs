using System;
using System.Collections.Generic;

namespace CourseSignUp.Domain.Model
{
    public class Student : BaseModel
    {
        public Student()
        {

        }
        public Student(string name, DateTime birthDate)
        {
            Name = name;
            BirthDate = birthDate;
        }

        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public override bool Equals(object obj)
        {
            var student = obj as Student;
            return student != null &&
                   Name == student.Name &&
                   BirthDate == student.BirthDate;
        }

        public override int GetHashCode()
        {
            var hashCode = 568674016;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + BirthDate.GetHashCode();
            return hashCode;
        }
    }
}
