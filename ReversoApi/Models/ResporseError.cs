namespace ReversoApi.Models
{
    public class ResponseError
    {
        public bool Error { get; set; }
        public bool Success { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }
    }

    [System.Obsolete("Use ResponseError instead.")]
    public class ResporseError : ResponseError
    {
    }
}
