namespace ReversoApi.Models.Word
{
    public class TranslateWordRequest(Language @from, Language to) 
        : TranslateRequestBase(@from, to)
    {
        public string Word { get; set; }
        public string WordPos { get; set; } = "0";
        public readonly string PageUrl = "0";
        public readonly string PageTitle = "0";
        public readonly string ReversoPage = "null";
	}
}
