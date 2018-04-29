// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using OWLeagueBot.Models;
//
//    var team = Team.FromJson(jsonString);

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OWLeagueBot.Models.Responses
{
    public partial class TeamResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("homeLocation")]
        public string HomeLocation { get; set; }

        [JsonProperty("primaryColor")]
        public string PrimaryColor { get; set; }

        [JsonProperty("secondaryColor")]
        public string SecondaryColor { get; set; }

        [JsonProperty("accounts")]
        public AccountResponse[] Accounts { get; set; }

        [JsonProperty("abbreviatedName")]
        public string AbbreviatedName { get; set; }

        [JsonProperty("addressCountry")]
        public string AddressCountry { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("players")]
        public PlayerResponse[] Players { get; set; }

        [JsonProperty("secondaryPhoto")]
        public string SecondaryPhoto { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("placement")]
        public long Placement { get; set; }

        [JsonProperty("advantage")]
        public long Advantage { get; set; }

        [JsonProperty("ranking")]
        public RecordResponse Ranking { get; set; }

        [JsonProperty("schedule")]
        public ScheduleResponse[] Schedule { get; set; }

        [JsonProperty("aboutUrl")]
        public string AboutUrl { get; set; }

        [JsonProperty("owl_division")]
        public long OwlDivision { get; set; }

        public ScheduleResponse[] GetFutureMatches() => Schedule.Where(t => t.State.Equals("PENDING")).OrderBy(t => t.EndDate).ToArray();
        public ScheduleResponse[] GetConcludedMatches() => Schedule.Where(t => t.State.Equals("CONCLUDED")).OrderBy(s => s.EndDate).ToArray();
        public ScheduleResponse GetLastMatchup(int opponentId) => GetConcludedMatches().Where(m => m.Competitors[1].Id == opponentId || m.Competitors[0].Id == opponentId).ToArray().Last();
    }
}
