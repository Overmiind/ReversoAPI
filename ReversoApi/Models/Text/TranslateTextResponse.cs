namespace ReversoApi.Models.Text
{
    public class TranslateTextResponse : ResponseError
    {
        public string Translation { get; set; }
        public object FavoriteId { get; set; }
        public string DirectionFrom { get; set; }
        public string DirectionTo { get; set; }
        public bool IsDirectionChanged { get; set; }
    }
}
