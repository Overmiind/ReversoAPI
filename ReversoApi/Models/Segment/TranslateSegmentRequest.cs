using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoApi.Models.Segment
{
    public class TranslateSegmentRequest: TranslateRequestBase
    {
        public TranslateSegmentRequest(Language @from, Language to) : base(@from, to)
        {
        }
    }
}
