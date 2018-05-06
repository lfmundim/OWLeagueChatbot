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
        private readonly Settings _settings;
        private readonly IBucketExtension _bucket;
        private readonly IOWLFilter _owlFilter;
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly ISchedulerExtension _scheduler;
        private readonly IBroadcastExtension _broadcast;
        private readonly ILogger _logger;

        public SelectMainTeamReceiver(
            IContextManager contextManager,
            IContactExtension contactService,
            ISender sender,
            ILogger logger,
            IBucketExtension bucket,
            IOWLFilter owlFilter,
            ICarouselBuilder carouselBuilder,
            ISchedulerExtension scheduler,
            IBroadcastExtension broadcast,
            Settings settings) : base(contextManager, contactService, sender, logger, bucket, scheduler, broadcast, owlFilter, settings) 
        {
            _contextManager = contextManager;
            _contactService = contactService;
            _sender = sender;
            _bucket = bucket;
            _owlFilter = owlFilter;
            _carouselBuilder = carouselBuilder;
            _scheduler = scheduler;
            _broadcast = broadcast;
            _settings = settings;
            _logger = logger;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            var teamListName = "Alert_" + message.Content.ToString().Split('_')[1];
            await _broadcast.UpdateDistributionListAsync(teamListName, message.From.ToIdentity(), cancellationToken);
            await _sender.SendMessageAsync("Awesome! I added you to my list for that team and will notify you of any matches 30mins earlier!", message.From, cancellationToken);
            await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
            await _sender.SendMessageAsync("What can I help you with?", message.From, cancellationToken);
        }
    }
}
