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
            var formRequest = new Dictionary<string, string>
            {
                { "from", request.From },
                { "to", request.Tos },
                { "subject", request.Subject },
                { "text", request.Content }
            };

            var response = await _client.PostRequest<MailGunResponse>(_host, _path, _headers, formRequest);

            return new EmailResponse { IsSuccessStatusCode = response.IsSuccessStatusCode, Message = response.Message };
        }
    }
}
