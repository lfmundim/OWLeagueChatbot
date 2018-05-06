using Lime.Protocol;
using Lime.Protocol.Serialization;
using Newtonsoft.Json;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Broadcast;
using Take.Blip.Client.Extensions.Bucket;

namespace OWLeagueBot.Tests
{
    public static class UnitTestBuilder
    {
        public static Settings GetSettings()
        {
            var settings = new Settings();
            using (StreamReader r = new StreamReader("../../../settings.json"))
            {
                string json = r.ReadToEnd();
                settings = JsonConvert.DeserializeObject<Settings>(json);
            }
            return settings;
        }

        public static CancellationToken GetCancellationToken() => new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

        public static ISender GetSender()
        {
            var settings = GetSettings();
            var builder = new BlipClientBuilder().UsingAccessKey(settings.BotIdentifier, settings.BotAccessKey).UsingRoutingRule(Lime.Messaging.Resources.RoutingRule.Instance);
            return builder.Build();
        }

        public static Node GetUserNode() => new Node() { Domain = "UnitTests.io", Name = "testUser" };
        public static Node GetBotNode() => new Node() { Domain = "msging.net", Name = "blizzconbrasil" };
        public static IOWLApiService GetClient() => OWLApiFactory.Build("https://api.overwatchleague.com");
    }
}
