using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OWLeagueBot.Models.Responses
{
    public partial class TeamsResponse
    {
        [JsonProperty("competitors")]
        public CompetitorElement[] Competitors { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        //[JsonProperty("owl_divisions")]
        //public OwlDivision[] OwlDivisions { get; set; }
    }

    public partial class CompetitorElement
    {
        [JsonProperty("competitor")]
        public Competitor Competitor { get; set; }
    }

    public partial class Competitor
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("homeLocation")]
        public string HomeLocation { get; set; }

        //[JsonProperty("primaryColor")]
        //public string PrimaryColor { get; set; }

        //[JsonProperty("secondaryColor")]
        //public string SecondaryColor { get; set; }

        [JsonProperty("accounts")]
        public AccountResponse[] Accounts { get; set; }

        [JsonProperty("abbreviatedName")]
        public string AbbreviatedName { get; set; }

        //[JsonProperty("addressCountry")]
        //public string AddressCountry { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("owl_division")]
        public long OwlDivision { get; set; }
    }

    //public partial class OwlDivision
    //{
    //    [JsonProperty("id")]
    //    public string Id { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("abbrev")]
    //    public string Abbrev { get; set; }
    //}
}
