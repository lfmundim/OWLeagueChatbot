using Lime.Messaging.Contents;
using Lime.Protocol;
using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Services
{
    public class CarouselBuilder : ICarouselBuilder
    {
        private readonly IOWLFilter _owlFilter;
        private readonly ISender _sender;

        public CarouselBuilder(IOWLFilter owlFilter, ISender sender)
        {
            _owlFilter = owlFilter;
            _sender = sender;
        }
        public async Task SendOnboardingTeamCarouselAsync(Message message, CancellationToken cancellationToken)
        {
            var division = GetDivisionFromText(message.Content.ToString());
            var divisionTeams = await _owlFilter.GetTeamsByDivisionAsync(division);
            var itemList = GetItemListFromTeams(divisionTeams, Flow.Onboarding);
            var menu = new DocumentCollection
            {
                Items = itemList.ToArray(),
                Total = itemList.Count,
                ItemType = DocumentSelect.MediaType
            };
            var menuMessage = new Message()
            {
                Content = menu,
                To = message.From
            };
            await _sender.SendMessageAsync(menuMessage, cancellationToken);
        }

        private List<DocumentSelect> GetItemListFromTeams(CompetitorElement[] divisionTeams, Flow flowIdentity)
        {
            var tag = GetTagFromFlow(flowIdentity);
            var buttonText = GetSingleButtonTextFromFlow(flowIdentity);
            var itemList = new List<DocumentSelect>();

            foreach (CompetitorElement c in divisionTeams)
            {
                var item = new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = c.Competitor.Name,
                            Text = c.Competitor.HomeLocation,
                            Uri = new Uri(c.Competitor.Logo),
                            AspectRatio = MyConstants.FacebookCarouselAspectRatio
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer{ Value = PlainText.Parse(buttonText) } ,
                            Value = new DocumentContainer{ Value = tag + c.Competitor.AbbreviatedName }
                        }
                    }
                };
                itemList.Add(item);
            }
            return itemList;
        }

        private string GetTagFromFlow(Flow flowIdentity)
        {
            switch (flowIdentity)
            {
                case Flow.Onboarding:
                    return "#SelectMainTeam_";
                case Flow.MainMenu:
                    return "#MainMenu_";
                case Flow.MySchedule:
                    return "#MySchedule_";
                case Flow.News:
                    return "#News_";
                case Flow.Alerts:
                    return "#Alerts_";
                case Flow.Standings:
                    return "#Standings_";
                default:
                    return "";
            }
        }

        private string GetSingleButtonTextFromFlow(Flow flowIdentity)
        {
            switch (flowIdentity)
            {
                case Flow.Onboarding:
                    return "This one!";
                default:
                    return "";
            }
        }

        private static DivisionIds GetDivisionFromText(string text)
        {
            switch (text)
            {
                case "79":
                    return DivisionIds.AtlanticDivision;
                case "80":
                    return DivisionIds.PacificDivision;
                default:
                    return DivisionIds.None;
            }
        }
    }
}
