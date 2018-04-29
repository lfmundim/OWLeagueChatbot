using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OWLeagueBot
{
    /// <summary>
    /// Define a simple settings file that can be injected to the receivers.
    /// This type and its values must be registered in the application.json file.
    /// </summary>
    public class Settings
    {
        [JsonProperty("OWLEndpoint")]
        public string OWLEndpoint { get; set; }
        [JsonProperty("OWLLogo")]
        public string OWLLogo { get; set; }
        [JsonProperty("OWLTwitch")]
        public string OWLTwitch { get; set; }
        [JsonProperty("BroadcastDomain")]
        public string BroadcastDomain { get; set; }
        [JsonProperty("BroadcastMessageIdPrefix")]
        public string BroadcastMessageIdPrefix { get; set; }
    }
}
