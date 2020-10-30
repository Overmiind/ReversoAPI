using System;
using System.Collections.Generic;
using System.Text;

namespace ReversoApi.Models.Text
{
    public class TranslateTextRequest: TranslateRequestBase
    {
        public TranslateTextRequest(Language @from, Language to) : base(@from, to)
        {
        }
    }
}
