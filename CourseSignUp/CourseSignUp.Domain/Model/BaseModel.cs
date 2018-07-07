using System.ComponentModel.DataAnnotations;

namespace CourseSignUp.Domain.Model
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
    }
}
