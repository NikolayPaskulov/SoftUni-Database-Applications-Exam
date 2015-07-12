
namespace Import_Leagues_and_Teams_from_XML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using List_Team_Names;

    class ImportLeaguesAndTeamsFromXml
    {
        static void Main()
        {
            var context = new FootballEntities();
            var xmlDoc = XDocument.Load(@"..\..\leagues-and-teams.xml");
            var leaguesNodes = xmlDoc.XPathSelectElements("/leagues-and-teams/league");
            int leagueCounter = 1;

            foreach (var league in leaguesNodes)
            {
                Console.WriteLine(string.Format("Processing league #{0} ...", leagueCounter));
                var leagueName = league.Descendants("league-name").FirstOrDefault();
                var teams = league.Descendants("teams").FirstOrDefault();
                var currentLeague = new League();
                if (leagueName != null)
                {
                    var currentLeagueInDb = context.Leagues.FirstOrDefault(l => l.LeagueName == leagueName.Value);
                    if (currentLeagueInDb == null)
                    {
                        currentLeague.LeagueName = leagueName.Value;
                        Console.WriteLine("Created league: {0}", leagueName.Value);
                        context.Leagues.Add(currentLeague);
                    }
                    else
                    {
                        currentLeague = currentLeagueInDb;
                        Console.WriteLine("Existing league: {0}", leagueName.Value);
                    }

                }

                var leagueTeams = currentLeague.Teams;

                if (teams != null && teams.Descendants("team").FirstOrDefault() != null)
                {
                    foreach (var team in teams.Descendants("team"))
                    {
                        var currentTeam = new Team();
                        var teamName = team.Attribute("name").Value;
                        var teamCountryAttr = team.Attribute("country");
                        var teamInDb = context.Teams.FirstOrDefault(t => t.TeamName == teamName);

                        if (teamInDb != null && teamCountryAttr != null &&
                            teamInDb.Country.CountryName != teamCountryAttr.Value)
                        {
                            teamInDb = null;
                        }

                        if (teamInDb == null)
                        {
                            var teamCountryCode = (teamCountryAttr != null)
                                ? context.Countries.FirstOrDefault(c => c.CountryName == teamCountryAttr.Value)
                                : null;

                            currentTeam.TeamName = teamName;
                            if (teamCountryCode != null)
                            {
                                currentTeam.CountryCode = teamCountryCode.CountryCode;
                                
                            }
                            Console.WriteLine("Created team: {0}", teamName);
                            context.Teams.Add(currentTeam);
                        }
                        else
                        {
                            currentTeam = teamInDb;
                            Console.WriteLine("Existing team: {0}", teamName);
                        }

                        if (leagueTeams.Contains(currentTeam))
                        {
                            Console.WriteLine("Existing team: {0}", teamName);
                        }
                        else
                        {
                            leagueTeams.Add(currentTeam);
                            Console.WriteLine("Added team to league: {0} to league {1}", teamName, currentLeague.LeagueName);
                        }
                    }
                }
                leagueCounter++;
                Console.WriteLine();
            }

            context.SaveChanges();
        }
    }
}
