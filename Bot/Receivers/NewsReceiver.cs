using System.Threading;
using System.Threading.Tasks;
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
    public class NewsReceiver : BaseMessageReceiver
    {
        private readonly IOWLFilter _owlFilter;
        private readonly ICarouselBuilder _carouselBuilder;
        private readonly ISender _sender;
        private readonly IQuickReplyBuilder _quickReplyBuilder;

        public NewsReceiver(IContextManager contextManager, IContactExtension contactService, ISender sender, ILogger logger, IDevActionHandler devActionHandler, IOWLFilter owlFilter, ICarouselBuilder carouselBuilder, IQuickReplyBuilder quickReplyBuilder) : base(contextManager, contactService, sender, logger, devActionHandler)
        {
            _owlFilter = owlFilter;
            _carouselBuilder = carouselBuilder;
            _sender = sender;
            _quickReplyBuilder = quickReplyBuilder;
        }

        protected override async Task ReceiveMessageAsync(Message message, Contact contact, UserContext userContext, CancellationToken cancellationToken)
        {
            var news = await _owlFilter.GetNewsAsync();
            var newsCarousel = await _carouselBuilder.GetNewsCarouselAsync(news, cancellationToken);
            newsCarousel.To = message.From;
            await _sender.SendMessageAsync("Here are the league's latest news:", message.From, cancellationToken);
            await _sender.SendMessageAsync(newsCarousel, cancellationToken);
            var back = _quickReplyBuilder.GetBackQuickReply();
            back.To = message.From;
            await _sender.SendDelayedComposingAsync(message.From, 2000, cancellationToken);
            await _sender.SendMessageAsync(back, cancellationToken);
        }
    }
}