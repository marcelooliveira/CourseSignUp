using System;

namespace CourseSignUp.Core.Model
{
    public class Student : BaseModel
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
