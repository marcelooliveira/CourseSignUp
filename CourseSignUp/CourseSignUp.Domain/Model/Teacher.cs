using System;

namespace CourseSignUp.Domain.Model
{
    public class Teacher : BaseModel
    {
        public Teacher()
        {

        }

        public Teacher(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
