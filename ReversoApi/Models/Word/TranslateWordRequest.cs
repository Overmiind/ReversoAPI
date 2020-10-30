namespace ReversoApi.Models.Word
{
    public class TranslateWordRequest: TranslateRequestBase
    {
        public string Word { get; set; }
        public string WordPos { get; set; } = "0";
        public const string PageUrl = "0";
        public const string PageTitle = "0";
        public const string ReversoPage = "null";

        public TranslateWordRequest(Language @from, Language to) : base(@from, to)
        {
        }
    }
}
