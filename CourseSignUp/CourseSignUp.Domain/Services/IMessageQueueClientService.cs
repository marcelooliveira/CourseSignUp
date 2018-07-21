using CourseSignUp.Domain.Model;

namespace CourseSignUp.Domain.Services
{
    public interface IMessageQueueClientService
    {
        void EnqueueSignUpMessage(SignUpInput input);
    }
}