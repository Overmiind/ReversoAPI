namespace ReversoApi.Models.Segment
{
    public class TranslateSegmentRequest(Language @from, Language to) 
        : TranslateRequestBase(@from, to)
    {
	}
}
