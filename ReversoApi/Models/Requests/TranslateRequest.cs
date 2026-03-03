using System;

namespace ReversoApi.Models.Requests
{
    /// <summary>
    /// Public translation request for ReversoClient.
    /// </summary>
    public sealed class TranslateRequest
    {
        public string? Word { get; init; }
        public string? Sentence { get; init; }
        public string WordPos { get; init; } = "0";
        public required Language From { get; init; }
        public required Language To { get; init; }

        public string? GetWordText()
        {
            return string.IsNullOrWhiteSpace(Word) ? null : Word.Trim();
        }

        public string? GetSentenceText()
        {
            return string.IsNullOrWhiteSpace(Sentence) ? null : Sentence.Trim();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(GetWordText()) && string.IsNullOrWhiteSpace(GetSentenceText()))
            {
                throw new ArgumentException("At least one field must be provided: Word or Sentence.");
            }

            if (string.IsNullOrWhiteSpace(WordPos))
            {
                throw new ArgumentException("WordPos cannot be null or empty.", nameof(WordPos));
            }
        }
    }
}
