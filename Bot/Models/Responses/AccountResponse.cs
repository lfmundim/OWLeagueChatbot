using Newtonsoft.Json;

namespace OWLeagueBot.Models.Responses
{
    public partial class AccountResponse
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }
    }
}