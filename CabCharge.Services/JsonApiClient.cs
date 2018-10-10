using CabCharge.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CabCharge.Services
{
    public class JsonApiClient : IJsonApiClient
    {
        private IHttpClientFactory _httpClientFactory;

        public JsonApiClient(IHttpClientFactory httpClientFactory)
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

            var content = JsonConvert.SerializeObject(request, Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync($"{host}{path}", byteContent);
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
