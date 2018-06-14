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
        private readonly ILogger _logger;
        private readonly IContactExtension _contactService;
        private readonly IContextManager _contextManager;
        private readonly IQuickReplyBuilder _quickReplyBuilder;

        public DefaultMessageReceiver(IContextManager contextManager, 
                                      IContactExtension contactService, 
                                      ISender sender, 
                                      ILogger logger, 
                                      IDevActionHandler devActionHandler,
                                      IQuickReplyBuilder quickReplyBuilder) : base(contextManager, contactService, sender, logger, devActionHandler) 
        {
            _sender = sender;
            _logger = logger;
            _contactService = contactService;
            _contextManager = contextManager;
            _quickReplyBuilder = quickReplyBuilder;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            await _sender.SendMessageAsync("I'm sorry, I'm just a robot, I didn't quite catch that 😓", message.From, cancellationToken);
            var back = _quickReplyBuilder.GetBackQuickReply();
            back.To = message.From;
            await _sender.SendMessageAsync(back, cancellationToken);
        }
    }
}
