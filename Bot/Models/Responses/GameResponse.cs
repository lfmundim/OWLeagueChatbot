using Newtonsoft.Json;

namespace OWLeagueBot.Models.Responses
{
    public partial class GameResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("points")]
        public long[] Points { get; set; }

        [JsonProperty("attributes")]
        public GameAttributes Attributes { get; set; }
    }

    public partial class GameAttributes
    {
        [JsonProperty("map")]
        public string Map { get; set; }

        [JsonProperty("mapScore")]
        public MapScore MapScore { get; set; }
    }

    public partial class MapScore
    {
        [JsonProperty("team1")]
        public long Team1 { get; set; }

        [JsonProperty("team2")]
        public long Team2 { get; set; }
    }

    public partial class Score
    {
        [JsonProperty("value")]
        public long Value { get; set; }
    }
}