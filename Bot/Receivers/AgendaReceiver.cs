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
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly IQuickReplyBuilder _quickReplyBuilder;

        public AgendaReceiver(IContextManager contextManager,
                                  IContactExtension contactService,
                                  ISender sender,
                                  ILogger logger,
                                  ICarouselBuilder carouselBuilder,
                                  IQuickReplyBuilder quickReplyBuilder,
                                  IDevActionHandler devActionHandler) : base(contextManager, contactService, sender, logger, devActionHandler)
        {
            _contextManager = contextManager;
            _contactService = contactService;
            _sender = sender;
            _logger = logger;
            _carouselBuilder = carouselBuilder;
            _quickReplyBuilder = quickReplyBuilder;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            var context = await _contextManager.GetUserContextAsync(message.From, cancellationToken);
            var carousel = await _carouselBuilder.GetAgendaCarouselAsync(context.AlertTeams, cancellationToken);
            carousel.To = message.From;
            var textMessage = new Message()
            {
                Content = PlainText.Parse("Here's your overview for this week:"),
                To = message.From
            };
            await _sender.SendMessageAsync(textMessage, cancellationToken);
            await _sender.SendMessageAsync(carousel, cancellationToken);
            await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
            var back = _quickReplyBuilder.GetBackQuickReply();
            back.To = message.From;
            await _sender.SendMessageAsync(back, cancellationToken);
        }
    }
}
