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
        private readonly IChatbotFlowService _flowService;

        public OnboardingReceiver(IContextManager contextManager, 
                                  IContactExtension contactService, 
                                  ISender sender,
                                  ILogger logger,
                                  IDevActionHandler devActionHandler,
                                  IChatbotFlowService flowService) : base(contextManager, contactService, sender, logger, devActionHandler)
        {
            _flowService = flowService;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            await _flowService.SendOnboardingFlowAsync(userContext, message, cancellationToken);
        }
    }
}
