using CabCharge.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CabCharge.Services
{
    public interface IFormDataApiClient
    {
        Task<TResponse> PostRequest<TResponse>(string host, string apiPath, IDictionary<string, string> headers, IDictionary<string, string> datas) where TResponse : ApiClientResponse, new();
    }
}
