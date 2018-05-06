using Lime.Messaging.Contents;
using Lime.Protocol;
using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using OWLeagueBot.Services;
using System;
using System.Threading.Tasks;
using Take.Blip.Client.Extensions.Scheduler;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Extensions
{
    public static class SchedulerExtensions
    {
        public static async Task<bool> UpdateBroadcastMessagesAsync(this ISchedulerExtension schedulerExtension, IOWLFilter _owlFilter, Settings settings)
        {
            try
            {
                foreach (TeamIds t in MyConstants.AllTeams)
                {
                    var futureMatches = await _owlFilter.GetFutureMatchesAsync(t);
                    foreach (ScheduleResponse s in futureMatches)
                    {
                        var message = BuildBroadcastMessage(t, s, settings);
                        var time = GetDateTimeOffset(s);
                        await schedulerExtension.ScheduleMessageAsync(message, time);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static DateTimeOffset GetDateTimeOffset(ScheduleResponse s)
        {
            var dateTime = s.StartDate.ConvertLongIntoDateTime();
            var alertTime = dateTime.AddMinutes(MyConstants.DefaultBroadcastOverhead);
            return alertTime;
        }

        private static Message BuildBroadcastMessage(TeamIds t, ScheduleResponse s, Settings settings)
        {
            var message = new Message($"{settings.BroadcastMessageIdPrefix}{s.Id.ToString()}")
            {
                To = GetListFromTeamId(t, settings),
                Content = GetMessageFromScheduledEvent(s, t, settings)
            };
            return message;
        }

        private static Document GetMessageFromScheduledEvent(ScheduleResponse s, TeamIds t, Settings settings)
        {
            var carousel = new DocumentCollection()
            {
                ItemType = DocumentSelect.MediaType,
                Items = new DocumentSelect[]
                {
                    new DocumentSelect
                    {
                        Header = new DocumentContainer
                        {
                            Value = new MediaLink
                            {
                                AspectRatio = MyConstants.FacebookCarouselAspectRatio,
                                Uri = new Uri(settings.OWLLogo),
                                Title = s.Competitors[0].Name + " X " + s.Competitors[1].Name,
                                Text = $"{s.StartDate.ConvertLongIntoDateTime().ToShortDateString()} - {s.StartDate.ConvertLongIntoDateTime().ToShortTimeString()} (GMT)"
                            }
                        },
                        Options = new DocumentSelectOption[]
                        {
                            new DocumentSelectOption
                            {
                                Label = new DocumentContainer
                                    {
                                        Value = new WebLink
                                        {
                                            Uri = new Uri(settings.OWLTwitch),
                                            Title ="💻Watch!"
                                        }
                                    }
                            }
                        }
                    }
                }
            };
            return carousel;
        }

        private static Node GetListFromTeamId(TeamIds t, Settings settings)
        {
            var suffix = string.Empty;
            switch (t)
            {
                case TeamIds.BostonUprising:
                    suffix = "BOS";
                    break;
                case TeamIds.DallasFuel:
                    suffix = "DAL";
                    break;
                case TeamIds.FloridaMayhem:
                    suffix = "FLA";
                    break;
                case TeamIds.HoustonOutlaws:
                    suffix = "HOU";
                    break;
                case TeamIds.LondonSpitfire:
                    suffix = "LON";
                    break;
                case TeamIds.LosAngelesGladiators:
                    suffix = "GLA";
                    break;
                case TeamIds.LosAngelesValiant:
                    suffix = "VAL";
                    break;
                case TeamIds.NewYorkExcelsior:
                    suffix = "NYE";
                    break;
                case TeamIds.PhiladelphiaFusion:
                    suffix = "PHI";
                    break;
                case TeamIds.SanFranciscoShock:
                    suffix = "SFS";
                    break;
                case TeamIds.SeoulDynasty:
                    suffix = "SEO";
                    break;
                case TeamIds.ShanghaiDragons:
                default:
                    suffix = "SHD";
                    break;
            }
            return Node.Parse($"Alert_{suffix}{settings.BroadcastDomain}");
        }
    }
}
