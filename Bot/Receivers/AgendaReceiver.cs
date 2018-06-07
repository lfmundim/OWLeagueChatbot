using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Contents;
using Lime.Messaging.Resources;
using Lime.Protocol;
using NLog;
using OWLeagueBot.Extensions;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using Take.Blip.Client;
using Take.Blip.Client.Extensions.Contacts;

namespace OWLeagueBot.Receivers
{
    public class AgendaReceiver : BaseMessageReceiver
    {
        private readonly IContextManager _contextManager;
        private readonly IContactExtension _contactService;
        private readonly ISender _sender;
        private readonly ILogger _logger;
        private readonly IChatbotFlowService _flowService;

        public AgendaReceiver(IContextManager contextManager,
                                  IContactExtension contactService,
                                  ISender sender,
                                  ILogger logger,
                                  IChatbotFlowService flowService,
                                  IDevActionHandler devActionHandler) : base(contextManager, contactService, sender, logger, devActionHandler)
        {
            _contextManager = contextManager;
            _contactService = contactService;
            _sender = sender;
            _logger = logger;
            _flowService = flowService;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            await _flowService.SendAgendaFlowAsync(userContext, message, cancellationToken);
        }
    }
}
