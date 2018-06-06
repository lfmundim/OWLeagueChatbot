using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OWLeagueBot.Models
{
    public class UserContext : Document
    {
        public static MediaType MEDIA_TYPE = MediaType.Parse("application/vnd.lime.owleaguebot.usercontext+json");
        public UserContext() : base(MEDIA_TYPE)
        {
            FirstInteraction = true;
            MainTeam = string.Empty;
            TeamDivision = string.Empty;
            AlertTeams = new List<string>();
        }
        [DataMember(Name = "mainteam")]
        public string MainTeam { get; set; }
        [DataMember(Name = "teamdivision")]
        public string TeamDivision { get; set; }
        [DataMember(Name = "firstinteraction")]
        public bool FirstInteraction { get; set; }
        [DataMember(Name = "alertteams")]
        public List<string> AlertTeams { get; set; }
    }
}
