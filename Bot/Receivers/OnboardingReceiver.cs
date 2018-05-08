using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Resources;
using Lime.Protocol;
using NLog;
using OWLeagueBot.Extensions;
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
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly IQuickReplyBuilder _quickReplyBuilder;

        public OnboardingReceiver(IContextManager contextManager, 
                                  IContactExtension contactService, 
                                  ISender sender,
                                  ILogger logger,
                                  ICarouselBuilder carouselBuilder,
                                  IQuickReplyBuilder quickReplyBuilder, 
                                  IDevActionHandler devActionHandler) : base(contextManager, contactService, sender, logger, devActionHandler)
        {
            _contextManager = contextManager;
            _contactService = contactService;
            _sender = sender;
            _logger = logger;
            _carouselBuilder = carouselBuilder;
            _quickReplyBuilder = quickReplyBuilder;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            var text = message.Content.ToString();
            if (userContext.FirstInteraction)
            {
                await _sender.SendMessageAsync("Hey there! I’m Emily! I’m here to help you keep track of what is going on in the Overwatch League!", message.From, cancellationToken);
                await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                var menu = _quickReplyBuilder.GetDivisionQuickReply(Flow.Onboarding, cancellationToken);
                menu.To = message.From;
                await _sender.SendMessageAsync(menu, cancellationToken);
                userContext.FirstInteraction = false;
                await _contextManager.SetUserContextAsync(message.From, userContext, cancellationToken);
            }
            else //if (!text.Equals("#Onboarding_0"))
            {
                userContext.TeamDivision = text.Split('_')[1];
                await _contextManager.SetUserContextAsync(message.From, userContext, cancellationToken);
                await _sender.SendMessageAsync("And which team is it?", message.From, cancellationToken);
                var division = GetDivisionFromText(userContext.TeamDivision);
                var carousel = await _carouselBuilder.GetOnboardingTeamCarouselAsync(division, cancellationToken);
                carousel.To = message.From;
                await _sender.SendMessageAsync(carousel, cancellationToken);
            }
        }
    }
}
