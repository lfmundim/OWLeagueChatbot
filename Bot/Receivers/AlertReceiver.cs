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

        public AlertReceiver(IContextManager contextManager, 
                             IContactExtension contactService, 
                             ISender sender, 
                             ILogger logger, 
                             IDevActionHandler devActionHandler) : base(contextManager, contactService, sender, logger, devActionHandler)
        {
            _contextManager = contextManager;
            _contactService = contactService;
            _sender = sender;
            _logger = logger;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
