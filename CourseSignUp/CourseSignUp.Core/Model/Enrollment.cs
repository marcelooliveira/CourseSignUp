﻿using System;

namespace CourseSignUp.Core.Model
{
    public class Enrollment : BaseModel
    {
        public Enrollment(Course course, Student student)
        {
            Course = course;
            Student = student;
        }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
