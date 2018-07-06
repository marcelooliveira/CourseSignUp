using System;

namespace CourseSignUp.Domain.Model
{
    public class Course : BaseModel
    {
        public string Name { get; set; }
        public int MaxStudentCount { get; set; }
    }
}
