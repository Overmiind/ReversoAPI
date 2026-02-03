namespace ReversoApi.Models
{
    public class TranslateRequestBase(Language @from, Language to)
	{
        public string Source { get; set; }
        public string Direction => $"{@from.ToString().ToLower()}-{to.ToString().ToLower()}";
        public string DeviceId { get; init; }  = "0";
        public string UiLang { get; init; } = "ru";
        public string Origin { get; init; } = "chromeextension";
        public string AccessToken { get; init; } = "";
        public string AppId { get; init; } = "0";
	}
}
