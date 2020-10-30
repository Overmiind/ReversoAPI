using System.Threading.Tasks;
using ReversoApi;
using ReversoApi.Models;
using ReversoApi.Models.Segment;
using ReversoApi.Models.Text;
using ReversoApi.Models.Word;
using Xunit;

namespace ReversoTests
{
    public class ReversoServiceTests
    {
        [Fact]
        public async Task ShouldTranslateWord()
        {
            var service = new ReversoService();

            var result = await service.TranslateWord(new TranslateWordRequest(Language.En, Language.Ru)
            {
                Word = "Influence"
            });

            TranslationNotEmpty(result);
        }

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

            var result = await service.TranslateText(new TranslateTextRequest(Language.En, Language.Ru)
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

            var result = await service.TranslateText(new TranslateTextRequest(Language.En, Language.Ru)
            {
                Source = ""
            });

            Assert.True(result.Error);
            Assert.Equal("Source is empty", result.Message);
        }


        private static void TranslationNotEmpty(TranslateWordResponce result)
        {
            Assert.True(result.Success);
            Assert.False(result.Error);
            
            Assert.NotEmpty(result.Sources);
            Assert.NotEmpty(result.Sources[0].Translations);
            Assert.NotEmpty(result.Sources[0].Translations[0].Contexts);
        }
    }
}
