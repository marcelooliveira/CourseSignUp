using CourseSignUp.Domain.Model;
using System;

namespace CourseSignUp.Domain.Repositories
{
    public interface IStudentRepository
    {
        Student Save(string name, DateTime birthDate);
    }
}