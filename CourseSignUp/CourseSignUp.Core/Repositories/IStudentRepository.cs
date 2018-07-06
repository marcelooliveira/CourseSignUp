using CourseSignUp.Core.Model;
using System;

namespace CourseSignUp.Core.Repositories
{
    public interface IStudentRepository
    {
        Student Save(string name, DateTime birthDate);
    }
}