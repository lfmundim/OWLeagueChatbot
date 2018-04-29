using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Services
{
    public class OWLFilter : IOWLFilter
    {
        private readonly IOWLApiService _owlApiService;

        public OWLFilter(IOWLApiService owlApiService)
        {
            _owlApiService = owlApiService;
        }
        public async Task<ScheduleResponse> GetLastMatchupAsync(TeamIds mainTeamId, TeamIds secondTeamId)
        {
            var team = await _owlApiService.GetTeamAsync((int)mainTeamId);
            var lastMatchup = team.GetLastMatchup((int)secondTeamId);
            return lastMatchup;
        }

        public async Task<ScheduleResponse> GetNextMatchAsync(TeamIds teamId)
        {
            var team = await _owlApiService.GetTeamAsync((int)teamId);
            var futureMatches = team.GetFutureMatches();
            var nextMatch = futureMatches.FirstOrDefault();
            return nextMatch;
        }

        public async Task<ScheduleResponse[]> GetFutureMatchesAsync(TeamIds teamId)
        {
            var team = await _owlApiService.GetTeamAsync((int)teamId);
            var futureMatches = team.GetFutureMatches();
            
            return futureMatches;
        }

        public async Task<NewsResponse> GetNewsAsync()
        {
            var news = await _owlApiService.GetNewsAsync();
            return news;
        }

        public async Task<RankingResponse> GetRankingAsync()
        {
            var ranking = await _owlApiService.GetRankingAsync();
            return ranking;
        }

        public async Task<CompetitorElement[]> GetTeamsByDivisionAsync(DivisionIds divisionId)
        {
            var teams = await _owlApiService.GetTeamsAsync();
            var divisionTeams = teams.Competitors.Where(c => c.Competitor.OwlDivision == (int)divisionId).ToArray();
            return divisionTeams;
        }
    }
}
