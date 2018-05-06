using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Contents;
using Lime.Messaging.Resources;
using Lime.Protocol;
using NLog;
using OWLeagueBot.Extensions;
using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using OWLeagueBot.Services;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Broadcast;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Contacts;
using Take.Blip.Client.Extensions.Scheduler;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Receivers
{
    public abstract class BaseMessageReceiver : IMessageReceiver
    {
        private readonly IContactExtension _contactService;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IContextManager _contextManager;
        private readonly IDevActionHandler _devActionHandler;

        public BaseMessageReceiver(IContextManager contextManager,
                                   IContactExtension contactService,
                                   ISender sender,
                                   ILogger logger,
                                   IDevActionHandler devActionHandler)
        {
            _contactService = contactService;
            _sender = sender;
            _logger = logger;
            _contextManager = contextManager;
            _devActionHandler = devActionHandler;
        }
        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var userContext = await _contextManager.GetUserContextAsync(message.From, cancellationToken);
                var contact = await _contactService.GetAsync(message.From, cancellationToken);

                if (message.Content.ToString().Trim().Contains("#DEVACTION#"))
                {
                    await _devActionHandler.HandleAsync(message, cancellationToken);
                }
                else
                {
                    await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                    await ReceiveMessageAsync(message, contact, userContext, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        protected abstract Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken);
    }
}
