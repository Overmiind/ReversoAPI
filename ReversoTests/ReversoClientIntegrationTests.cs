using System.Threading.Tasks;
using ReversoApi;
using ReversoApi.Models;
using ReversoApi.Models.Requests;
using ReversoApi.Models.Responses;
using Xunit;

namespace ReversoTests
{
    public class ReversoClientIntegrationTests
    {
        [Fact]
        public async Task TranslateAsync_Sentence_ShouldReturnTranslation()
        {
            using var client = new ReversoClient();
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Sentence = "Private methods should be small"
            };

            var result = await client.TranslateAsync(request);

            Assert.Equal(TranslationKind.Sentence, result.Kind);
            Assert.False(result.Error);
            Assert.False(string.IsNullOrWhiteSpace(result.Translation));
        }

        [Fact]
        public async Task TranslateAsync_Segment_ShouldReturnSources()
        {
            using var client = new ReversoClient();
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Sentence = "private methods"
            };

            var result = await client.TranslateAsync(request);

            Assert.Equal(TranslationKind.Segment, result.Kind);
            Assert.True(result.Success, $"Segment translation failed. Error={result.Error}, Message={result.Message}, SourcesCount={result.Sources?.Count ?? 0}");
        }

        [Fact]
        public async Task TranslateAsync_Word_ShouldReturnSources()
        {
            using var client = new ReversoClient();
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Word = "influence",
                Sentence = "They all wanted to influence the decision."
            };

            var result = await client.TranslateAsync(request);

            Assert.Equal(TranslationKind.Word, result.Kind);
            Assert.True(result.Success, $"Word translation failed. Error={result.Error}, Message={result.Message}, SourcesCount={result.Sources?.Count ?? 0}");
        }

        [Fact]
        public async Task TranslateSegmentAsync_ShouldReturnSources()
        {
            using var client = new ReversoClient();
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Word = "influence",
                Sentence = "They all wanted to influence the decision."
            };

            var result = await client.TranslateSegmentAsync(request);

            Assert.Equal(TranslationKind.Segment, result.Kind);
            Assert.False(result.Error);
            Assert.NotNull(result.Sources);
            Assert.NotEmpty(result.Sources!);
            Assert.NotEmpty(result.Sources![0].Translations);
        }
    }
}
