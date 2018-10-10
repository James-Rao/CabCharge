using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CabCharge.Models;
using Microsoft.Extensions.Configuration;

namespace CabCharge.Services
{
    public class SendGridEmailer : ISendGridEmailer
    {
        private readonly IJsonApiClient _client;
        private readonly string _host;
        private readonly string _path;
        private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();

        public SendGridEmailer(IConfiguration configuration, IJsonApiClient client)
        {
            _host = configuration["SendGrid:Host"];
            _path = configuration["SendGrid:Path"];
            _headers.Add(configuration["SendGrid:AuthKey"], configuration["SendGrid:AuthValue"]);
            _client = client;
        }

        public async Task<EmailResponse> SendEmail(EmailRequest request)
        {
            var jsonObj = new SendGridRequest();
            var tos = request.Tos.Split(';', StringSplitOptions.RemoveEmptyEntries);
            jsonObj.Personalizations = new Personalization[1];
            jsonObj.Personalizations[0] = new Personalization { To = new EmailPro[tos.Length] };
            for (int i = 0; i < tos.Length; ++i)
            {
                jsonObj.Personalizations[0].To[i] = new EmailPro { Email = tos[i] };
            }
            jsonObj.From = new EmailPro { Email = request.From };
            jsonObj.Subject = request.Subject;
            jsonObj.Content = new Content[1];
            jsonObj.Content[0] = new Content { Type = "text/plain", Value = request.Content };

            var response = await _client.PostRequest<SendGridRequest, SendGridResponse>(_host, _path, _headers, jsonObj);

            return new EmailResponse { IsSuccessStatusCode = response.IsSuccessStatusCode, Message = response.Message };
        }
    }
}
