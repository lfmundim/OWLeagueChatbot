using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Contents;
using Lime.Messaging.Resources;
using Lime.Protocol;
using Lime.Protocol.Network;
using NLog;
using OWLeagueBot.Extensions;
using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using OWLeagueBot.Services;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Broadcast;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Contacts;
using Take.Blip.Client.Extensions.Directory;
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
                var contact = await GetContact(message, cancellationToken);

                if (message.Content.ToString().Trim().Contains("#DEVACTION#"))
                {
                    await _devActionHandler.HandleAsync(message, cancellationToken);
                }
                else
                {
                    if(message.Content.ToString().Trim().Contains("Main Menu"))
                    {
                        message.Content = PlainText.Parse("#MainMenu_");
                    }
                    await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
                    await ReceiveMessageAsync(message, contact, userContext, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await _sender.SendMessageAsync("Oops, something went wrong...", message.From, cancellationToken);   
            }
        }

        private async Task<Contact> GetContact(Message message, CancellationToken cancellationToken)
        {
            Contact contact;
            try
            {
                contact = await _contactService.GetAsync(message.From.ToIdentity(), cancellationToken);
            }
            catch(LimeException lex)
            {
                var directory = new DirectoryExtension(_sender);
                var account = await directory.GetDirectoryAccountAsync(message.From.ToIdentity(), cancellationToken);
                contact = await _contactService.GetAsync(message.From.ToIdentity(), cancellationToken);
            }
            return contact;
        }

        private async Task<Contact> GetContact(Message message, CancellationToken cancellationToken)
        {
            Contact contact;
            try
            {
                contact = await _contactService.GetAsync(message.From.ToIdentity(), cancellationToken);
            }
            catch(LimeException lex)
            {
                var directory = new DirectoryExtension(_sender);
                var account = await directory.GetDirectoryAccountAsync(message.From.ToIdentity(), cancellationToken);
                contact = await _contactService.GetAsync(message.From.ToIdentity(), cancellationToken);
            }
            return contact;
        }

        protected abstract Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken);
    }
}
