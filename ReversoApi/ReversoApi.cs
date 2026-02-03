using RestSharp;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ReversoTests")]
namespace ReversoApi
{
	internal sealed class ReversoApi : IDisposable
    {
        public const string Url = "https://cps.reverso.net/api2";
        private readonly RestClient _client;

        public ReversoApi()
        {
            _client = new RestClient(Url);
        }

		public void Dispose()
		{
			_client.Dispose();
		}

		public async Task<RestResponse<T2>> SendPostRequest<T2>(string url, object model)
        {
            var request = new RestRequest(url) {Method = Method.Post};
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddJsonBody(model);

            var result = await _client.ExecutePostAsync<T2>(request);

            return result;
        }
    }
}
