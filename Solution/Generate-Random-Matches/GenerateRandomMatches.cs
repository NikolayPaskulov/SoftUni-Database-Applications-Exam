

namespace Generate_Random_Matches
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using List_Team_Names;

    class GenerateRandomMatches
    {
        static void Main()
        {
            var context = new FootballEntities();
            var teams = context.Teams;
            var xmlDoc = XDocument.Load(@"..\..\generate-matches.xml");
            var randomMatches = xmlDoc.XPathSelectElements("/generate-random-matches/generate");
            int count = 5;
            int maxGoals = 10;
            int processingCounter = 1;
            string league = "no league";
            DateTime startDate = new DateTime(2000, 1, 1);
            DateTime endDate = new DateTime(2015, 12, 31);

            foreach (var match in randomMatches)
            {
                Console.WriteLine("Processing request #{0} ...", processingCounter);
                processingCounter++;
                if (match.Attribute("generate-count") != null)
                {
                    count = int.Parse(match.Attribute("generate-count").Value);
                }

                if (match.Attribute("max-goals") != null)
                {
                    maxGoals = int.Parse(match.Attribute("max-goals").Value);
                }

                if (match.Descendants("start-date").FirstOrDefault() != null)
                {
                    startDate = FormatDateTimeAsReq(match.Descendants("start-date").First().Value);
                }

                if (match.Descendants("end-date").FirstOrDefault() != null)
                {
                    endDate = FormatDateTimeAsReq(match.Descendants("end-date").First().Value);
                }

                if (match.Descendants("league").FirstOrDefault() != null)
                {
                    league = match.Descendants("league").First().Value;
                }

                for (int i = 0; i < count; i++)
                {
                    var currentMatch = new TeamMatch();
                    DateTime matchDate = GenerateRandomDateInRange(startDate, endDate);
                    int homeTeamScores = GenerateRandomNumber(maxGoals);
                    int awayTeamScores = GenerateRandomNumber(maxGoals);
                    int homeTeamId = 0;
                    int awayTeamId = 0;
                    do
                    {
                        homeTeamId = GenerateRandomNumber(teams.Count());
                        awayTeamId = GenerateRandomNumber(teams.Count());
                    } while (homeTeamId == awayTeamId);

                    currentMatch.AwayGoals = awayTeamScores;
                    currentMatch.AwayTeamId = awayTeamId;
                    currentMatch.HomeGoals = homeTeamScores;
                    currentMatch.HomeTeamId = homeTeamId;

                    currentMatch.MatchDate = matchDate;

                    if (league != "no league")
                    {
                        var currentLeague = context.Leagues.FirstOrDefault(l => l.LeagueName == league);
                        if (currentLeague != null)
                        {
                            currentMatch.League = currentLeague;
                        }
                    }

                    var homeTeamName = context.Teams.Find(homeTeamId).TeamName;
                    var awayTeamName = context.Teams.Find(awayTeamId).TeamName;

                    PrintOnConsole(homeTeamName, awayTeamName, homeTeamScores, awayTeamScores, 
                        league, matchDate);

                    context.TeamMatches.Add(currentMatch);
                }
                
            }
            context.SaveChanges();
        }


        public static DateTime GenerateRandomDateInRange(DateTime startDate, DateTime endDate)
        {
            var random = new Random();

            var day = random.Next(startDate.Day, endDate.Day);
            var month = random.Next(startDate.Month, endDate.Month);
            var year = random.Next(startDate.Year, endDate.Year);

            return new DateTime(year, month, day);
        }

        public static int GenerateRandomNumber(int max)
        {
            var random = new Random();

            return random.Next(0, max);
        }

        public static DateTime FormatDateTimeAsReq(string dt)
        {

            var date = dt.Split('-');
            string newDate = "";
            if (date[0].Length == 1)
            {
                newDate = "0" + String.Join("-", date);
            }
            else
            {
                newDate = String.Join("-", date);
            }

            return DateTime.ParseExact(
                newDate,
                "dd-MMM-yyyy",
                CultureInfo.InvariantCulture);
        }

        public static void PrintOnConsole(string homeTeamName, string awayTeamName, int homeTeamScore, 
                                          int awayTeamScore, string league, DateTime date)
        {

            var stringDate = date.ToString("dd-MMM-yyyyy");
            Console.WriteLine("{0}: {1} - {2}: {3}-{4} ({5})", stringDate, homeTeamName, awayTeamName, homeTeamScore, awayTeamScore,
                league);

        }
    }
}
