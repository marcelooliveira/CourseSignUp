using System;
using System.Collections.Generic;
using System.Text;

namespace CourseSignUp.Core.Model
{
    public class SignUpInput
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
