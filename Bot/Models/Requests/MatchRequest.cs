using Newtonsoft.Json;

namespace OWLeagueBot.Models.Requests
{
    public class MatchRequest
    {
        [JsonProperty("teamid")]
        public int teamId { get; set; } 
    }
}