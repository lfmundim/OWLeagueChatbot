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

namespace OWLeagueBot.Receivers
{
    public class AlertReceiver : BaseMessageReceiver
    {
        private readonly IContextManager _contextManager;
        private readonly IContactExtension _contactService;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IBucketExtension _bucket;
        private readonly ISchedulerExtension _scheduler;
        private readonly IBroadcastExtension _broadcast;
        private readonly IOWLFilter _owlFilter;
        private readonly Settings _settings;

        public AlertReceiver(IContextManager contextManager, 
                             IContactExtension contactService, 
                             ISender sender, 
                             ILogger logger, 
                             IBucketExtension bucket,
                             ISchedulerExtension scheduler, 
                             IBroadcastExtension broadcast, 
                             IOWLFilter owlFilter, 
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
            _settings = settings;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
