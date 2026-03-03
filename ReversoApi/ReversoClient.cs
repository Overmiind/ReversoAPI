using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ReversoApi.Internal;
using ReversoApi.Models;
using ReversoApi.Models.Requests;
using ReversoApi.Models.Responses;
using ReversoApi.Models.Text;

namespace ReversoApi
{
    /// <summary>
    /// Client for Reverso translation API.
    /// </summary>
    public sealed class ReversoClient : IDisposable
    {
        private readonly ReversoApi _api;
        private readonly ReversoSegmentApi _segmentApi;

        public ReversoClient()
        {
            _api = new ReversoApi();
            _segmentApi = new ReversoSegmentApi();
        }

        public void Dispose()
        {
            _api.Dispose();
            _segmentApi.Dispose();
        }

        /// <summary>
        /// Translates text and routes by token count in request.Word or request.Sentence.
        /// 1 word => TranslateWord, 2-3 words => TranslateSimple, more than 3 words => TranslateText.
        /// If Word is provided, Sentence is treated as context for single-word mode.
        /// </summary>
        public async Task<TranslateResponse> TranslateAsync(TranslateRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            request.Validate();

            var word = request.GetWordText();
            var sentence = request.GetSentenceText();
            var direction = ReversoRequestFactory.BuildDirection(request.From, request.To);
            var input = word ?? sentence ?? throw new InvalidOperationException("Input is empty after validation.");
            var wordsCount = CountWords(input);

            if (wordsCount <= 1)
            {
                return await TranslateWordAsync(input, sentence, request.WordPos, direction, cancellationToken);
            }

            if (wordsCount <= 3)
            {
                return await TranslateSegmentAsync(input, direction, cancellationToken);
            }

            return await TranslateSentenceAsync(input, direction, cancellationToken);
        }

        /// <summary>
        /// Experimental API for better translation with explicit source sentence and target word.
        /// Uses newer version of reverso api 
        /// </summary>
        public async Task<TranslateResponse> TranslateSegmentAsync(TranslateRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            request.Validate();

            var word = request.GetWordText();
            var sentence = request.GetSentenceText();

            if (string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentException("Word is required for experimental segment translation.", nameof(request));
            }

            if (string.IsNullOrWhiteSpace(sentence))
            {
                throw new ArgumentException("Sentence is required for experimental segment translation.", nameof(request));
            }

            var direction = ReversoRequestFactory.BuildDirection(request.From, request.To);
            var query = new Dictionary<string, string>
            {
                ["direction"] = direction,
                ["source"] = sentence,
                ["word"] = word,
                ["wordPos"] = request.WordPos
            };

            var response = await _segmentApi.SendGetRequest<TranslatedResponse>("TranslateSegment", query, cancellationToken);
            EnsureSuccess(response, "TranslateSegment");

            var data = response.Data ?? throw new InvalidOperationException("TranslateSegment response body is empty.");
            return new TranslateResponse
            {
                Kind = TranslationKind.Segment,
                Input = word,
                Sources = data.Sources,
                Error = data.Error,
                Message = data.Message,
                Success = data.Success
            };
        }

        private async Task<TranslateResponse> TranslateSentenceAsync(string input, string direction, CancellationToken cancellationToken)
        {
            var payload = new TranslateTextApiRequest
            {
                Source = input,
                Direction = direction
            };

            var response = await _api.SendPostRequest<TranslateTextResponse>("TranslateText", payload, cancellationToken);
            EnsureSuccess(response, "TranslateText");

            var data = response.Data ?? throw new InvalidOperationException("TranslateText response body is empty.");
            return new TranslateResponse
            {
                Kind = TranslationKind.Sentence,
                Input = input,
                Translation = data.Translation,
                DirectionFrom = data.DirectionFrom,
                DirectionTo = data.DirectionTo,
                IsDirectionChanged = data.IsDirectionChanged,
                Error = data.Error,
                Message = data.Message,
                Success = data.Success
            };
        }

        private async Task<TranslateResponse> TranslateWordAsync(string input, string? sentence, string wordPos, string direction, CancellationToken cancellationToken)
        {
            var payload = new TranslateWordApiRequest
            {
                Source = sentence ?? input,
                Word = input,
                WordPos = ResolveWordPos(sentence, input, wordPos),
                Direction = direction
            };

            var response = await _api.SendPostRequest<TranslatedResponse>("TranslateWord", payload, cancellationToken);
            EnsureSuccess(response, "TranslateWord");

            var data = response.Data ?? throw new InvalidOperationException("TranslateWord response body is empty.");

            return new TranslateResponse
            {
                Kind = TranslationKind.Word,
                Input = input,
                Sources = data.Sources,
                Error = data.Error,
                Message = data.Message,
                Success = data.Success
            };
        }

        private async Task<TranslateResponse> TranslateSegmentAsync(string input, string direction, CancellationToken cancellationToken)
        {
            var payload = new TranslateTextApiRequest
            {
                Source = input,
                Direction = direction
            };

            var response = await _api.SendPostRequest<TranslatedResponse>("TranslateSimple", payload, cancellationToken);
            EnsureSuccess(response, "TranslateSimple");

            var data = response.Data ?? throw new InvalidOperationException("TranslateSimple response body is empty.");

            return new TranslateResponse
            {
                Kind = TranslationKind.Segment,
                Input = input,
                Sources = data.Sources,
                Error = data.Error,
                Message = data.Message,
                Success = data.Success
            };
        }

        private static int CountWords(string text)
        {
            return text
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Length;
        }

        private static bool IsEmptyResult(TranslatedResponse data)
        {
            return data.Error || data.Sources is null || data.Sources.Count == 0;
        }

        private static string ResolveWordPos(string? sentence, string word, string currentWordPos)
        {
            if (!string.Equals(currentWordPos, "0", StringComparison.Ordinal) || string.IsNullOrWhiteSpace(sentence))
            {
                return currentWordPos;
            }

            var index = sentence.IndexOf(word, StringComparison.OrdinalIgnoreCase);
            return index >= 0 ? index.ToString() : currentWordPos;
        }

        private static Language ParseDirectionFrom(string direction)
        {
            var parts = direction.Split('-', 2, StringSplitOptions.TrimEntries);
            return Enum.Parse<Language>(parts[0], true);
        }

        private static Language ParseDirectionTo(string direction)
        {
            var parts = direction.Split('-', 2, StringSplitOptions.TrimEntries);
            return Enum.Parse<Language>(parts[1], true);
        }

        private static void EnsureSuccess<T>(ApiHttpResponse<T> response, string endpoint)
        {
            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException($"Request to '{endpoint}' failed with status '{response.StatusCode}'. Error: {response.ErrorMessage}");
            }
        }
    }
}
