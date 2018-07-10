using System;

namespace CourseSignUp.Domain.Model
{
    public class Course : BaseModel
    {
        public Course()
        {

        }

        public Course(string code, string name, int maxStudentCount, int teacherId)
        {
            Code = code;
            Name = name;
            MaxStudentCount = maxStudentCount;
            TeacherId = teacherId;
        }

        public string Name { get; set; }
        public int MaxStudentCount { get; set; }
        public virtual int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int StudentCount { get; set; }
        public DateTime? MinBirthdate { get; set; }
        public DateTime? MaxBirthdate { get; set; }
        public long BirthdateTickSum { get; set; }
    }
}
