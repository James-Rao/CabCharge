using CabCharge.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Threading.Tasks;
using System;

namespace CabCharge.Services
{
    public class MailGunEmailer : IMailGunEmailer
    {
        private readonly IFormDataApiClient _client;
        private readonly string _host;
        private readonly string _path;
        private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();

        public MailGunEmailer(IConfiguration configuration, IFormDataApiClient client)
        {
            _host = configuration["MailGun:Host"];
            _path = configuration["MailGun:Path"];
            _headers.Add(configuration["MailGun:AuthKey"], configuration["MailGun:AuthValue"]);
            _client = client;
        }

        public async Task<EmailResponse> SendEmail(EmailRequest request)
        {
            var mgRequest = new MailGunRequest {  FormData = new Dictionary<string, string>()
            {
                { "from", request.From },
                { "to", request.Tos },
                { "subject", request.Subject },
                { "text", request.Content }
            }
            };

            var response = await _client.PostRequest<Dictionary<string, string>, MailGunResponse >(_host, _path, _headers, mgRequest.FormData);
            return new EmailResponse { IsSuccessed = response.IsSuccessStatusCode, Reason = response.ReasonPhrase, Message = response.Message };
        }
    }
}
