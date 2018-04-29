using Newtonsoft.Json;

namespace OWLeagueBot.Models.Responses
{
    public partial class RecordResponse
    {
        [JsonProperty("matchWin")]
        public long MatchWin { get; set; }

        [JsonProperty("matchLoss")]
        public long MatchLoss { get; set; }

        [JsonProperty("matchDraw")]
        public long MatchDraw { get; set; }

        [JsonProperty("matchBye")]
        public long MatchBye { get; set; }

        [JsonProperty("gameWin")]
        public long GameWin { get; set; }

        [JsonProperty("gameLoss")]
        public long GameLoss { get; set; }

        [JsonProperty("gameTie")]
        public long GameTie { get; set; }

        [JsonProperty("gamePointsFor")]
        public long GamePointsFor { get; set; }

        [JsonProperty("gamePointsAgainst")]
        public long GamePointsAgainst { get; set; }

        [JsonProperty("streak")]
        public string Streak { get; set; }

        [JsonProperty("streakNum")]
        public long StreakNum { get; set; }
    }
}