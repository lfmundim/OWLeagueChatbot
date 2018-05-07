﻿using System;
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
        private readonly ILogger _logger;
        private readonly IContactExtension _contactService;
        private readonly IContextManager _contextManager;
        private readonly INLPService _nlpService;

        public DefaultMessageReceiver(IContextManager contextManager, 
                                      IContactExtension contactService, 
                                      ISender sender, 
                                      ILogger logger, 
                                      IDevActionHandler devActionHandler,
                                      INLPService nlpService) : base(contextManager, contactService, sender, logger, devActionHandler) 
        {
            _sender = sender;
            _logger = logger;
            _contactService = contactService;
            _contextManager = contextManager;
            _nlpService = nlpService;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            await _nlpService.ProcessAsync(message, cancellationToken);
        }
    }
}
