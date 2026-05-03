using ToDoListChallenge.Domain.Services.Mail;

namespace ToDoListChallenge.Domain.Services
{
    public interface IMailService
    {
        void SendMail(MailMessage mailMessage);
    }
}
