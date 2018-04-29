using Newtonsoft.Json;

namespace OWLeagueBot.Models.Responses
{
    public partial class PlayerResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("homeLocation")]
        public string HomeLocation { get; set; }

        [JsonProperty("accounts")]
        public AccountResponse[] Accounts { get; set; }

        [JsonProperty("attributes")]
        public PlayerAttributes Attributes { get; set; }

        [JsonProperty("familyName")]
        public string FamilyName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("headshot")]
        public string Headshot { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("flags")]
        public object[] Flags { get; set; }

        [JsonProperty("aboutUrl")]
        public string AboutUrl { get; set; }
    }

    public partial class PlayerAttributes
    {
        [JsonProperty("hero_image")]
        public object HeroImage { get; set; }

        [JsonProperty("heroes")]
        public Hero[] Heroes { get; set; }

        [JsonProperty("player_number")]
        public long PlayerNumber { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }

    public partial class Hero
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}