using CabCharge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CabCharge.Services
{
    public class Emailer : IEmailer
    {
        private List<IEmailer> _emailers = new List<IEmailer>();

        public Emailer(IMailGunEmailer mailGun, ISendGridEmailer sendGrid)
        {
            _emailers.Add(mailGun);
            _emailers.Add(sendGrid);
        }

        public async Task<EmailResponse> SendEmail(EmailRequest request)
        {
            var lastResponse = new EmailResponse();
            foreach (var emailer in _emailers)
            {
                try
                {
                    lastResponse = await emailer.SendEmail(request);
                    if (lastResponse.IsSuccessed)
                    {
                        return lastResponse;
                    }
                }
                catch (System.Exception ex)
                {
                    //log
                    //throw;
                }
            }

            return lastResponse;
        }
    }
}
