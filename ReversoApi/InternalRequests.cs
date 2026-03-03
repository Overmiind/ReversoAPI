using ReversoApi.Models;

namespace ReversoApi.Internal
{
    internal sealed class TranslateTextApiRequest
    {
        public required string Source { get; init; }
        public required string Direction { get; init; }
        public string DeviceId { get; init; } = "0";
        public string UiLang { get; init; } = "ru";
        public string Origin { get; init; } = "chromeextension";
        public string AccessToken { get; init; } = string.Empty;
        public string AppId { get; init; } = "0";
    }

    internal sealed class TranslateWordApiRequest
    {
        public required string Source { get; init; }
        public required string Word { get; init; }
        public required string Direction { get; init; }
        public string WordPos { get; init; } = "0";
        public string PageUrl { get; init; } = "0";
        public string PageTitle { get; init; } = "0";
        public string ReversoPage { get; init; } = "null";
        public string DeviceId { get; init; } = "0";
        public string UiLang { get; init; } = "ru";
        public string Origin { get; init; } = "chromeextension";
        public string AccessToken { get; init; } = string.Empty;
        public string AppId { get; init; } = "0";
    }

    internal static class ReversoRequestFactory
    {
        public static string BuildDirection(Language from, Language to)
        {
            return $"{from.ToString().ToLowerInvariant()}-{to.ToString().ToLowerInvariant()}";
        }
    }
}
