using System;

namespace CourseSignUp.Domain.Model
{
    public class Course : BaseModel
    {
        public Course(string name, int maxStudentCount)
        {
            Name = name;
            MaxStudentCount = maxStudentCount;
        }

        public string Name { get; set; }
        public int MaxStudentCount { get; set; }
    }
}
