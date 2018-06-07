using NUnit.Framework;
using OWLeagueBot.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Tests.Models
{
    [TestFixture]
    public class EnumerationsUnitTests
    {
        [Test, Category("Short")]
        [TestCase("NYE", TeamIds.NewYorkExcelsior)]
        [TestCase("GLA", TeamIds.LosAngelesGladiators)]
        [TestCase("VAL", TeamIds.LosAngelesValiant)]
        [TestCase("DAL", TeamIds.DallasFuel)]
        [TestCase("HOU", TeamIds.HoustonOutlaws)]
        [TestCase("FLA", TeamIds.FloridaMayhem)]
        [TestCase("SEO", TeamIds.SeoulDynasty)]
        [TestCase("SHD", TeamIds.ShanghaiDragons)]
        [TestCase("SFS", TeamIds.SanFranciscoShock)]
        [TestCase("LON", TeamIds.LondonSpitfire)]
        [TestCase("PHI", TeamIds.PhiladelphiaFusion)]
        [TestCase("BOS", TeamIds.BostonUprising)]
        public void GetTeamIdFromTagTest(string tag, TeamIds returns)
        {
            var team = GetTeamIdFromTag(tag);

            team.ShouldBe(returns);
        }
        [Test, Category("Short")]
        [TestCase("80", DivisionIds.PacificDivision)]
        [TestCase("79", DivisionIds.AtlanticDivision)]
        [TestCase("42", DivisionIds.None)]
        public void GetDivisionFromTextTest(string tag, DivisionIds returns)
        {
            var division = GetDivisionFromText(tag);

            division.ShouldBe(returns);
        }
    }
}
