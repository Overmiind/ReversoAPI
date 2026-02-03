namespace ReversoApi.Models.Text
{
    public class TranslateTextRequest(Language @from, Language to) : TranslateRequestBase(@from, to)
    {
	}
}
