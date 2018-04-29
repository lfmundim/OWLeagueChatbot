using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using System.Diagnostics;
using Take.Blip.Client;
using OWLeagueBot.Services;
using Lime.Messaging.Resources;
using OWLeagueBot.Models;
using Take.Blip.Client.Extensions.Contacts;
using NLog;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Scheduler;
using Take.Blip.Client.Extensions.Broadcast;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Receivers
{
    /// <summary>
    /// Defines a class for handling messages. 
    /// This type must be registered in the application.json file in the 'messageReceivers' section.
    /// </summary>
    public class DefaultMessageReceiver : BaseMessageReceiver
    {
        private readonly ISender _sender;
        private readonly Settings _settings;
        private readonly IOWLFilter _owlFilter;
        private readonly ILogger _logger;
        private readonly IContactExtension _contactService;
        private readonly IContextManager _contextManager;
        private readonly IBucketExtension _bucket;
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly ISchedulerExtension _scheduler;
        private readonly IBroadcastExtension _broadcast;
        private readonly IQuickReplyBuilder _quickReplyBuilder;

        public DefaultMessageReceiver(IContextManager contextManager, 
                                      IContactExtension contactService, 
                                      ISender sender, 
                                      ILogger logger, 
                                      Settings settings,
                                      IBucketExtension bucket,
                                      IOWLFilter owlFilter,
                                      ICarouselBuilder carouselBuilder,
                                      ISchedulerExtension scheduler,
                                      IBroadcastExtension broadcast,
                                      IQuickReplyBuilder quickReplyBuilder) : base(contextManager, contactService, sender, logger, bucket, scheduler, broadcast, owlFilter, settings) 
        {
            _sender = sender;
            _settings = settings;
            _owlFilter = owlFilter;
            _logger = logger;
            _contactService = contactService;
            _contextManager = contextManager;
            _bucket = bucket;
            _carouselBuilder = carouselBuilder;
            _scheduler = scheduler;
            _broadcast = broadcast;
            _quickReplyBuilder = quickReplyBuilder;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            
        }
    }
}
