using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Resources;
using Lime.Protocol;
using NLog;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Broadcast;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Contacts;
using Take.Blip.Client.Extensions.Scheduler;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Receivers
{
    public class OnboardingReceiver : BaseMessageReceiver
    {
        private readonly IContextManager _contextManager;
        private readonly IContactExtension _contactService;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IBucketExtension _bucket;
        private readonly IOWLFilter _owlFilter;
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly ISchedulerExtension _scheduler;
        private readonly IBroadcastExtension _broadcast;
        private readonly IQuickReplyBuilder _quickReplyBuilder;
        private readonly Settings _settings;

        public OnboardingReceiver(IContextManager contextManager, 
                                  IContactExtension contactService, 
                                  ISender sender,
                                  ILogger logger,
                                  IBucketExtension bucket,
                                  IOWLFilter owlFilter, 
                                  ICarouselBuilder carouselBuilder,
                                  ISchedulerExtension scheduler,
                                  IBroadcastExtension broadcast, 
                                  IQuickReplyBuilder quickReplyBuilder, 
                                  Settings settings) : base(contextManager, contactService, sender, logger, bucket, scheduler, broadcast, owlFilter, settings)
        {
            _contextManager = contextManager;
            _contactService = contactService;
            _sender = sender;
            _logger = logger;
            _bucket = bucket;
            _scheduler = scheduler;
            _broadcast = broadcast;
            _owlFilter = owlFilter;
            _carouselBuilder = carouselBuilder;
            _quickReplyBuilder = quickReplyBuilder;
            _settings = settings;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            var text = message.Content.ToString();
            await SendDelayedComposing(message.From, 2000, cancellationToken);
            if (userContext.FirstInteraction)
            {
                await _sender.SendMessageAsync("Hey there! I’m Emily! I’m here to help you keep track of what is going on in the Overwatch League!", message.From, cancellationToken);
                await SendDelayedComposing(message.From, 2000, cancellationToken);
                var menu = _quickReplyBuilder.GetDivisionQuickReply(Flow.Onboarding, cancellationToken);
                menu.To = message.From;
                await _sender.SendMessageAsync(menu, cancellationToken);
                userContext.FirstInteraction = false;
                await _contextManager.SetUserContextAsync(message.From, userContext, cancellationToken);
            }
            else if (!text.Equals("#Onboarding_0"))
            {
                userContext.TeamDivision = text.Split('_')[1];
                await _sender.SendMessageAsync("And which team is it?", message.From, cancellationToken);
                var division = GetDivisionFromText(message.Content.ToString());
                var carousel = await _carouselBuilder.GetOnboardingTeamCarouselAsync(division, cancellationToken);
                carousel.To = message.From;
                await _sender.SendMessageAsync(carousel, cancellationToken);
            }
        }

        private static DivisionIds GetDivisionFromText(string text)
        {
            switch (text)
            {
                case "79":
                    return DivisionIds.AtlanticDivision;
                case "80":
                    return DivisionIds.PacificDivision;
                default:
                    return DivisionIds.None;
            }
        }
    }
}
