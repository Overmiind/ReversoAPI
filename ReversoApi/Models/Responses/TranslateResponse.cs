using System.Collections.Generic;

namespace ReversoApi.Models.Responses
{
    public enum TranslationKind
    {
        Word = 1,
        Segment = 2,
        Sentence = 3
    }

    /// <summary>
    /// Unified translation response for word, segment, and sentence requests.
    /// </summary>
    public sealed class TranslateResponse : ResponseError
    {
        public required TranslationKind Kind { get; init; }
        public required string Input { get; init; }
        public string? Translation { get; init; }
        public IList<Sources>? Sources { get; init; }
        public string? DirectionFrom { get; init; }
        public string? DirectionTo { get; init; }
        public bool? IsDirectionChanged { get; init; }
    }
}
