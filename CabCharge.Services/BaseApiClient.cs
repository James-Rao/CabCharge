using CabCharge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CabCharge.Services
{
    public abstract class BaseApiClient : IApiClient
    {
        protected IHttpClientFactory _httpClientFactory;

        public BaseApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResponse> PostRequest<TRequest, TResponse>(string host, string path, IDictionary<string, string> headers, TRequest request)
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

            HttpContent requestContent = SetConent(request);

            var response = await client.PostAsync($"{host}{path}", requestContent);
            if (response.IsSuccessStatusCode)
            {
                return new TResponse() { IsSuccessStatusCode = true };
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<TResponse>(responseContent);
                responseObj.IsSuccessStatusCode = false;
                responseObj.ReasonPhrase = response.ReasonPhrase;
                return responseObj;
            }
        }

        protected abstract HttpContent SetConent<TRequest>(TRequest request);
    }
}
