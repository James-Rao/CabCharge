﻿using CabCharge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CabCharge.Services
{
    public interface IApiClient
    {
        Task<TResponse> PostRequest<TRequest, TResponse>(string host, string apiPath, IDictionary<string, string> headers, TRequest request)
            where TResponse : ApiClientResponse, new();
    }
}
