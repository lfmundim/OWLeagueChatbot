using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OWLeagueBot.Tests
{
    public class Settings
    {
        [JsonProperty("identifier")]
        public string BotIdentifier;
        [JsonProperty("accessKey")]
        public string BotAccessKey;
    }
}
