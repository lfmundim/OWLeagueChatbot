using System;
using System.Collections.Generic;
using System.Text;

namespace OWLeagueBot.Models
{
    public class Enumerations
    {
        public enum TeamIds { DallasFuel = 4523, PhiladelphiaFusion = 4524, HoustonOutlaws = 4525,
                              BostonUprising = 4402, NewYorkExcelsior = 4403, SanFranciscoShock = 4404,
                              LosAngelesValiant = 4405, LosAngelesGladiators = 4406, FloridaMayhem = 4407,
                              ShanghaiDragons = 4408, SeoulDynasty = 4409, LondonSpitfire = 4410 };
        public enum DivisionIds { PacificDivision = 80, AtlanticDivision = 79, None = 0 };
        public enum Flow { Onboarding, MainMenu, MySchedule, News, Alerts, Standings};
        public static DivisionIds GetDivisionFromText(string text)
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
