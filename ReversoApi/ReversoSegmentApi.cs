using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReversoApi
{
    internal sealed class ReversoSegmentApi : IDisposable
    {
        public const string Url = "https://cps-api.reverso.net/Api/";
        private readonly HttpClient _client;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ReversoSegmentApi()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Url)
            };
            _client.DefaultRequestHeaders.TryAddWithoutValidation(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36");
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<ApiHttpResponse<TResponse>> SendGetRequest<TResponse>(
            string resource,
            IReadOnlyDictionary<string, string> queryParams,
            CancellationToken cancellationToken = default)
        {
            var query = await new FormUrlEncodedContent(queryParams).ReadAsStringAsync(cancellationToken);
            var requestUri = string.IsNullOrEmpty(query) ? resource : $"{resource}?{query}";

            HttpResponseMessage response;
            try
            {
                response = await _client.GetAsync(requestUri, cancellationToken);
            }
            catch (Exception ex)
            {
                return new ApiHttpResponse<TResponse>
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    IsSuccessful = false,
                    ErrorMessage = ex.Message
                };
            }

            TResponse? data = default;
            string? errorMessage = null;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    data = await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken);
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            else
            {
                errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
            }

            return new ApiHttpResponse<TResponse>
            {
                StatusCode = response.StatusCode,
                IsSuccessful = response.IsSuccessStatusCode && errorMessage is null,
                ErrorMessage = errorMessage,
                Data = data
            };
        }
    }
}

