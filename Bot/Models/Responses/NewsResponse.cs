using Newtonsoft.Json;

namespace OWLeagueBot.Models.Responses
{
    public partial class NewsResponse
    {
        [JsonProperty("blogs")]
        public Blog[] Blogs { get; set; }
    }

    public partial class Blog
    {
        [JsonProperty("blogId")]
        public long BlogId { get; set; }

        [JsonProperty("publish")]
        public long Publish { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("thumbnail")]
        public Thumbnail Thumbnail { get; set; }
    }

    public partial class Thumbnail
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("mimeType")]
        public string MimeType { get; set; }
    }

}