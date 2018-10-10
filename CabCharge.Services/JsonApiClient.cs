using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CabCharge.Services
{
    public class JsonApiClient : BaseApiClient, IJsonApiClient
    {
        public JsonApiClient(IHttpClientFactory httpClientFactory)
            :base(httpClientFactory)
        {
        }

        protected override HttpContent SetConent<TRequest>(TRequest request)
        {
            var content = JsonConvert.SerializeObject(request, Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}

