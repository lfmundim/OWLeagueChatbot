using Newtonsoft.Json;
using System.Linq;
using static OWLeagueBot.Models.Responses.BracketResponse;

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

        [JsonProperty("conclusionStrategy")]
        public string ConclusionStrategy { get; set; }

        [JsonProperty("winner")]
        public Winner Winner { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("games")]
        public GameResponse[] Games { get; set; }

        //[JsonProperty("bracket")]
        //public BracketResponse Bracket { get; set; }

        [JsonProperty("startDate")]
        public long? StartDate { get; set; }
    }
}