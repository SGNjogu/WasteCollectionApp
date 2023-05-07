using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ttnm.Infrastructure.Services.Helpers;

namespace ttnm.Infrastructure.Services.APIService
{
    public class RestService : IRestService
    {
        private HttpClient? _httpClient { get; set; }

        public RestService()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(Constants.BaseUrl);
            }
        }

        public async Task<T> GETRequest<T>(string uri)
        {
            try
            {
                var request = await _httpClient!.GetAsync(uri);

                var response = await request.Content.ReadAsStringAsync();

                if (request.IsSuccessStatusCode)
                {
                    return await JsonConverter.ReturnObjectFromJsonString<T>(response);
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> SENDRequest<T>(string uri, HttpMethod httpMethod, object? payload = null)
        {
            try
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                {
                    Method = httpMethod,
                    RequestUri = new Uri(uri, UriKind.Relative) // UriKind.Relative because httpclient already has baseUrl
                };

                if (payload != null)
                {
                    var json = await JsonConverter.ReturnJsonStringFromObject(payload);
                    httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                var request = await _httpClient!.SendAsync(httpRequestMessage);

                var response = await request.Content.ReadAsStringAsync();

                if (request.IsSuccessStatusCode)
                {
                    return await JsonConverter.ReturnObjectFromJsonString<T>(response);
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> POSTRequest<T>(string uri, object sender)
        {
            try
            {
                var request = await _httpClient!.PostAsJsonAsync(uri, sender);

                var response = await request.Content.ReadAsStringAsync();

                if (request.IsSuccessStatusCode)
                {
                    return await JsonConverter.ReturnObjectFromJsonString<T>(response);
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> POSTRequest<T>(string uri)
        {
            try
            {
                var request = await _httpClient!.PostAsync(uri, default);

                var response = await request.Content.ReadAsStringAsync();

                if (request.IsSuccessStatusCode)
                {
                    return await JsonConverter.ReturnObjectFromJsonString<T>(response);
                }
                else
                {
                    throw new Exception(response);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
