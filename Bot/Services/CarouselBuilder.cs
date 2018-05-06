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

        public CarouselBuilder(IOWLFilter owlFilter)
        {
            _owlFilter = owlFilter;
        }
        public async Task<Message> GetOnboardingTeamCarouselAsync(DivisionIds division, CancellationToken cancellationToken)
        {
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
                Content = menu
            };
            return menuMessage;
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
                case Flow.MySchedule:
                    return "#MySchedule_";
                case Flow.News:
                    return "#News_";
                case Flow.Alerts:
                    return "#Alerts_";
                case Flow.Standings:
                    return "#Standings_";
                case Flow.MainMenu:
                default:
                    return "#MainMenu_";
            }
        }

        private string GetSingleButtonTextFromFlow(Flow flowIdentity)
        {
            switch (flowIdentity)
            {
                case Flow.Onboarding:
                default:
                    return "This one!";
            }
        }
    }
}
