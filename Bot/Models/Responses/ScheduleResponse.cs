using Newtonsoft.Json;
using System.Linq;

namespace OWLeagueBot.Models.Responses
{
    public partial class ScheduleResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("competitors")]
        public Winner[] Competitors { get; set; }

        [JsonProperty("scores")]
        public Score[] Scores { get; set; }

        [JsonProperty("conclusionValue")]
        public long ConclusionValue { get; set; }

        [JsonProperty("conclusionStrategy")]
        public string ConclusionStrategy { get; set; }

        [JsonProperty("winner")]
        public Winner Winner { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("games")]
        public GameResponse[] Games { get; set; }

        [JsonProperty("clientHints")]
        public object[] ClientHints { get; set; }

        [JsonProperty("bracket")]
        public BracketResponse Bracket { get; set; }

        [JsonProperty("dateCreated")]
        public long DateCreated { get; set; }

        [JsonProperty("flags")]
        public object[] Flags { get; set; }

        [JsonProperty("handle")]
        public string Handle { get; set; }

        [JsonProperty("startDate")]
        public long? StartDate { get; set; }

        [JsonProperty("endDate")]
        public long? EndDate { get; set; }

        [JsonProperty("showStartTime")]
        public bool ShowStartTime { get; set; }

        [JsonProperty("showEndTime")]
        public bool ShowEndTime { get; set; }
    }
}