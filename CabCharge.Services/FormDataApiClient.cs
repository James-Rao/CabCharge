using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CabCharge.Services
{
    public class FormDataApiClient : BaseApiClient, IFormDataApiClient
    {
        public FormDataApiClient(IHttpClientFactory httpClientFactory)
            : base (httpClientFactory)
        {
        }

        protected override HttpContent SetConent<TRequest>(TRequest request)
        {
            var form = new MultipartFormDataContent
            {
                new FormUrlEncodedContent(request as Dictionary<string, string>)
            };
            form.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return form;
        }
    }
}


