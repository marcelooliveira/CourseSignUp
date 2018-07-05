using System;

namespace CourseSignUp.Core.Model
{
    public class Course : BaseModel
    {
        public string Name { get; set; }
        public int MaxStudentCount { get; set; }
    }
}
