using System.Threading.Tasks;
using ReversoApi.Models;
using ReversoApi.Models.Segment;
using ReversoApi.Models.Text;
using ReversoApi.Models.Word;

namespace ReversoApi
{
    public class ReversoService
    {
        private readonly ReversoApi _api = new ReversoApi();

        
        /// <summary>
        /// Translates simple sentence
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TranslateTextResponse> TranslateText(TranslateTextRequest request)
        {
            const string url = "TranslateText";
            var result = await Translate<TranslateTextResponse>(url, request);
            return result;
        }


        /// <summary>
        /// Translates only one word
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TranslateWordResponce> TranslateWord(TranslateWordRequest request)
        {
            const string url = "TranslateWord";

            request.Word ??= request.Source;
            request.Source ??= request.Word;

            var result = await Translate<TranslateWordResponce>(url, request);
            return result;
        }

        /// <summary>
        /// Translates small text fragment (on average three words)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TranslateWordResponce> TranslateSegment(TranslateSegmentRequest request)
        {
            const string url = "TranslateSimple";
            var result = await Translate<TranslateWordResponce>(url, request);

            return result;
        }


        private async Task<T> Translate<T>(string url, TranslateRequestBase request)
        {
            var result = await _api.SendPostRequest<T>(url, request);
            return result.Data;
        }
    }
}
