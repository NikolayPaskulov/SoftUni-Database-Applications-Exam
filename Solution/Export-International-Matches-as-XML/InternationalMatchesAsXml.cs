using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Export_International_Matches_as_XML
{
    using System.Globalization;
    using System.Xml.Linq;
    using List_Team_Names;

    class InternationalMatchesAsXml
    {
        static void Main()
        {
            var context = new FootballEntities();
            var internationalMatches = context.InternationalMatches
                .OrderBy(m => m.MatchDate)
                .ThenBy(m => m.Country1)
                .ThenBy(m => m.Country)
                .Select(m => new
                {
                    m.HomeCountryCode,
                    homeCountryName = m.Country1.CountryName,
                    m.AwayCountryCode,
                    awayCountryName = m.Country.CountryName,
                    m.MatchDate,
                    m.League,
                    m.HomeGoals,
                    m.AwayGoals
                })
                .ToList();

            var xmlRoot = new XElement("matches");
            foreach (var match in internationalMatches)
            {
                var xmlMatch = new XElement("match");
                if (match.MatchDate != null)
                {
                    DateTime date = DateTime.Parse(match.MatchDate.ToString());
                    if (date.Hour == 0 && date.Minute == 0)
                    {
                        var dateAttr = new XAttribute("date", date.ToString("dd-MMM-yyyy"));
                        xmlMatch.Add(dateAttr);
                    }
                    else
                    {
                        var dateAttr = new XAttribute("date", date.ToString("dd-MMM-yyyy hh:ss"));
                        xmlMatch.Add(dateAttr);
                    }

                }

                var homeCountry = new XElement("home-country", match.HomeCountryCode);
                var homeCountryCodeAtt = new XAttribute("code", match.HomeCountryCode);
                homeCountry.Add(homeCountryCodeAtt);
                xmlMatch.Add(homeCountry);

                var awayCountry = new XElement("home-country", match.awayCountryName);
                var awayCountryCodeAtt = new XAttribute("code", match.AwayCountryCode);
                awayCountry.Add(awayCountryCodeAtt);
                xmlMatch.Add(awayCountry);

                if (match.HomeGoals != null && match.AwayGoals != null)
                {
                    var scores = string.Format("{0}-{1}", match.HomeGoals, match.AwayGoals);
                    var scoreXml = new XElement("score", scores);
                    xmlMatch.Add(scoreXml);
                }

                if (match.League != null)
                {
                    var leagueXml = new XElement("league", match.League.LeagueName);
                    xmlMatch.Add(leagueXml);
                }

                xmlRoot.Add(xmlMatch);
            }
            var xmlDoc = new XDocument(xmlRoot);
            xmlDoc.Save("international-matches.xml");

            Console.WriteLine("File international-matches.xml was saved in bin/Debug");
        }
    }
}
