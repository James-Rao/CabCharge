using CabCharge.Models;
using System.Threading.Tasks;

namespace CabCharge.Services
{
    public interface IEmailer
    {
        Task<EmailResponse> SendEmail(EmailRequest request);
    }
}
