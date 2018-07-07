using System;

namespace CourseSignUp.Domain.Model
{
    public class Course : BaseModel
    {
        public Course()
        {

        }

        public Course(string code, string name, int maxStudentCount)
        {
            Code = code;
            Name = name;
            MaxStudentCount = maxStudentCount;
        }

        public string Name { get; set; }
        public int MaxStudentCount { get; set; }
    }
}
