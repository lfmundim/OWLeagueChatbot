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

namespace OWLeagueBot.Receivers
{
    public class SelectMainTeamReceiver : BaseMessageReceiver
    {
        private readonly IContextManager _contextManager;
        private readonly IContactExtension _contactService;
        private readonly ISender _sender;
        private readonly IBroadcastExtension _broadcast;
        private readonly ILogger _logger;
        private readonly ICarouselBuilder _carouselBuilder;

        public SelectMainTeamReceiver(
            IContextManager contextManager,
            IContactExtension contactService,
            ISender sender,
            ILogger logger,
            IBroadcastExtension broadcast,
            IDevActionHandler devActionHandler,
            ICarouselBuilder carouselBuilder) : base(contextManager, contactService, sender, logger, devActionHandler) 
        {
            _contextManager = contextManager;
            _contactService = contactService;
            _sender = sender;
            _broadcast = broadcast;
            _logger = logger;
            _carouselBuilder = carouselBuilder;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            if(message.Content.ToString().Contains("SelectMainTeam_"))
            {
                var teamTag = message.Content.ToString().Split('_')[1];
                var teamListName = "Alert_" + teamTag;
                await _broadcast.UpdateDistributionListAsync(teamListName, message.From.ToIdentity(), cancellationToken);
                userContext.MainTeam = teamTag;
                userContext.AlertTeams.Add(teamTag);
                await _sender.SendMessageAsync("Awesome! I added you to my list for that team and will notify you of any matches 30mins earlier!", message.From, cancellationToken);
            }
            await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
            await _sender.SendMessageAsync("What can I help you with?", message.From, cancellationToken);
            var carousel = _carouselBuilder.GetMainMenuCarousel();
            carousel.To = message.From;
            await _sender.SendMessageAsync(carousel, cancellationToken);
        }
    }
}
