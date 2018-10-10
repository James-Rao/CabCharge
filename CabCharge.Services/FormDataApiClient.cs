using CabCharge.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CabCharge.Services
{
    public class FormDataApiClient : IFormDataApiClient
    {
        private IHttpClientFactory _httpClientFactory;

        public FormDataApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResponse> PostRequest<TResponse>(string host, string path, IDictionary<string ,string> headers, IDictionary<string, string> datas)
            where TResponse : ApiClientResponse, new()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var form = new MultipartFormDataContent
            {
                new FormUrlEncodedContent(datas)
            };
            form.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.PostAsync($"{host}{path}", form);
            if (response.IsSuccessStatusCode)
            {
                return new TResponse() { IsSuccessStatusCode = true };
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<TResponse>(responseContent);
                responseObj.IsSuccessStatusCode = false;
                return responseObj;
            }
        }
    }
}
