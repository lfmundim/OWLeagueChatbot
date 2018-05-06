// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using OWLeagueBot.Models;
//
//    var ranking = Ranking.FromJson(jsonString);
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OWLeagueBot.Models.Responses
{

    public partial class RankingResponse
    {
        [JsonProperty("content")]
        public Content[] Content { get; set; }

        //[JsonProperty("totalMatches")]
        //public long TotalMatches { get; set; }

        //[JsonProperty("matchesConcluded")]
        //public long MatchesConcluded { get; set; }

        //[JsonProperty("playoffCutoff")]
        //public long PlayoffCutoff { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("competitor")]
        public TeamResponse Competitor { get; set; }

        [JsonProperty("placement")]
        public long Placement { get; set; }

        //[JsonProperty("advantage")]
        //public long Advantage { get; set; }

        [JsonProperty("records")]
        public RecordResponse[] Records { get; set; }
    }
}
