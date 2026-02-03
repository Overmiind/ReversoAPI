using ReversoApi;
using ReversoApi.Models;
using ReversoApi.Models.Segment;
using ReversoApi.Models.Text;
using ReversoApi.Models.Word;
using System.Threading.Tasks;
using Xunit;

namespace ReversoTests
{
    public class ReversoServiceTests
    {
        [Fact]
        public async Task ShouldTranslateSentence()
        {
            var service = new ReversoService();

            var result = await service.TranslateSegment(new TranslateSegmentRequest(Language.En, Language.Ru)
            {
                Source = "Private methods"
            });

            TranslationNotEmpty(result);
        }

        [Fact]
        public async Task ShouldTranslateText()
        {
            var service = new ReversoService();

            var result = await service.TranslateSentence(new TranslateTextRequest(Language.En, Language.Ru)
            {
                Source = "Private methods"
            });

            Assert.NotNull(result.Translation);
            Assert.False(result.Error);
        }

        [Fact]
        public async Task ShouldThrowException()
        {
            var service = new ReversoService();

            var result = await service.TranslateSentence(new TranslateTextRequest(Language.En, Language.Ru)
            {
                Source = ""
            });

            Assert.True(result.Error);
            Assert.Equal("Source is empty", result.Message);
        }

        [Fact]
        public async Task ShouldTranslateWordWrong()
        {
            var service = new ReversoService();

            var result = await service.TranslateWord(new TranslateWordRequest(Language.En, Language.Ru)
            {
                Word = "Influence",
                Source = "They all wanted to influence the decision."
            });

            TranslationNotEmpty(result);
        }

        private static void TranslationNotEmpty(TranslatedResponse result)
        {
            Assert.True(result.Success);
            Assert.False(result.Error);
            
            Assert.NotEmpty(result.Sources);
            Assert.NotEmpty(result.Sources[0].Translations);
            Assert.NotEmpty(result.Sources[0].Translations[0].Contexts);
        }
    }
}
