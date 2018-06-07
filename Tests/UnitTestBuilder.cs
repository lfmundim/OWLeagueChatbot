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

        public static IChatbotFlowService GetFlowService()
        {
            var sender = GetSender();
            var owlFilter = new OWLFilter(GetClient());
            var carouselBuilder = new CarouselBuilder(owlFilter);
            var quickReplyBuilder = new QuickReplyBuilder();
            var broadcast = new BroadcastExtension(sender);
            var bucket = new BucketExtension(sender);
            var contextManager = new ContextManager(bucket);

            return new ChatbotFlowService
            (
                carouselBuilder,
                quickReplyBuilder,
                broadcast,
                contextManager,
                owlFilter,
                sender
            );
        }

        public static IContextManager GetContextManager()
        {
            var sender = GetSender();
            var bucket = new BucketExtension(sender);
            return new ContextManager(bucket);
        }

        public static Node GetUserNode() => new Node() { Domain = "UnitTests.io", Name = "testUser" };
        public static Node GetBotNode() => new Node() { Domain = "msging.net", Name = "blizzconbrasil" };
        public static IOWLApiService GetClient() => OWLApiFactory.Build("https://api.overwatchleague.com");
    }
}
