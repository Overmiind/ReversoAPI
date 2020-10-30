using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RestSharp;
[assembly:InternalsVisibleTo("ReversoTests")]
namespace ReversoApi
{
    internal class ReversoApi
    {
        public const string Url = "https://cps.reverso.net/api2";
        private readonly RestClient _client;
        public ReversoApi()
        {
            _client = new RestClient(Url);
        }

        public async Task<IRestResponse<T2>> SendPostRequest<T2>(string url, object model)
        {
            var request = new RestRequest(url) {Method = Method.POST};
            request.AddJsonBody(model);

            var result = await _client.ExecutePostAsync<T2>(request);

            return result;
        }
    }
}
