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
        public int StudentCount { get; set; }
        public DateTime? MinBirthdate { get; set; }
        public DateTime? MaxBirthdate { get; set; }
        public long BirthdateTickSum { get; set; }
    }
}
