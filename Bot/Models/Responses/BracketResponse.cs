using Newtonsoft.Json;

namespace OWLeagueBot.Models.Responses
{
    public partial class BracketResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("stage")]
        public BracketStage Stage { get; set; }

        [JsonProperty("thirdPlaceMatch")]
        public bool ThirdPlaceMatch { get; set; }

        [JsonProperty("allowDraw")]
        public bool AllowDraw { get; set; }
    }

    public partial class BracketStage
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("tournament")]
        public Tournament Tournament { get; set; }
    }

    public partial class Tournament
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("attributes")]
        public TournamentAttributes Attributes { get; set; }

        [JsonProperty("series")]
        public Series Series { get; set; }
    }

    public partial class TournamentAttributes
    {
        [JsonProperty("program")]
        public Program Program { get; set; }
    }

    public partial class Program
    {
        [JsonProperty("phase")]
        public string Phase { get; set; }

        [JsonProperty("season_id")]
        public string SeasonId { get; set; }

        [JsonProperty("stage")]
        public ProgramStage Stage { get; set; }
    }

    public partial class ProgramStage
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }
    }

    public partial class Series
    {
        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class Winner
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}