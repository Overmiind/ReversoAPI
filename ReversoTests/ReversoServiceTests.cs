using System;
using System.Threading.Tasks;
using ReversoApi;
using ReversoApi.Internal;
using ReversoApi.Models;
using ReversoApi.Models.Requests;
using Xunit;

namespace ReversoTests
{
    public class ReversoClientTests
    {
        [Fact]
        public void BuildDirection_ShouldFormatAsLowerCaseFromTo()
        {
            var result = ReversoRequestFactory.BuildDirection(Language.En, Language.Ru);

            Assert.Equal("en-ru", result);
        }

        [Fact]
        public void Validate_ShouldThrow_WhenWordAndSentenceAreMissing()
        {
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Word = "  ",
                Sentence = null
            };

            var ex = Assert.Throws<ArgumentException>(() => request.Validate());
            Assert.Equal("At least one field must be provided: Word or Sentence.", ex.Message);
        }

        [Fact]
        public void Validate_ShouldThrow_WhenWordPosIsEmpty()
        {
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Sentence = "hello",
                WordPos = ""
            };

            var ex = Assert.Throws<ArgumentException>(() => request.Validate());
            Assert.Equal("WordPos cannot be null or empty. (Parameter 'WordPos')", ex.Message);
        }

        [Fact]
        public void GetTextHelpers_ShouldTrimInput_AndReturnNullForWhitespace()
        {
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Word = "  influence  ",
                Sentence = "  this is context  "
            };

            Assert.Equal("influence", request.GetWordText());
            Assert.Equal("this is context", request.GetSentenceText());

            request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Word = "   ",
                Sentence = "\t"
            };

            Assert.Null(request.GetWordText());
            Assert.Null(request.GetSentenceText());
        }

        [Fact]
        public async Task TranslateAsync_ShouldThrow_WhenRequestIsNull()
        {
            using var client = new ReversoClient();

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.TranslateAsync(null!));
        }

        [Fact]
        public async Task TranslateSegmentAsync_ShouldThrow_WhenWordIsMissing()
        {
            using var client = new ReversoClient();
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Word = "",
                Sentence = "source sentence"
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => client.TranslateSegmentAsync(request));
            Assert.Equal("Word is required for experimental segment translation. (Parameter 'request')", ex.Message);
        }

        [Fact]
        public async Task TranslateSegmentAsync_ShouldThrow_WhenSentenceIsMissing()
        {
            using var client = new ReversoClient();
            var request = new TranslateRequest
            {
                From = Language.En,
                To = Language.Ru,
                Word = "influence",
                Sentence = ""
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => client.TranslateSegmentAsync(request));
            Assert.Equal("Sentence is required for experimental segment translation. (Parameter 'request')", ex.Message);
        }
    }
}
