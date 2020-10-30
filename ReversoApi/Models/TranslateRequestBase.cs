namespace ReversoApi.Models
{
    public class TranslateRequestBase
    {
        public const string DeviceId = "0";
        public const string UiLang = "ru";
        public const string Origin = "chromeextension";
        public const string AccessToken = "";
        public readonly string Direction;
        public string Source { get; set; }
        public const string AppId = "0";

        public TranslateRequestBase(Language from, Language to)
        {
            Direction = $"{from.ToString().ToLower()}-{to.ToString().ToLower()}";
        }
    }
}
