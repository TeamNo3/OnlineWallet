using System.Threading.Tasks;

namespace Services.EmailSender
{
    interface IEmailSender
    {
        Task SendAsync(IEmailMessage emailMessage);
    }
}
