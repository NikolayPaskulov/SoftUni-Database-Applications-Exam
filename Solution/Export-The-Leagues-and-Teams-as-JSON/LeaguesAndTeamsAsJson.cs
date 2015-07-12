
namespace Export_The_Leagues_and_Teams_as_JSON
{
    using List_Team_Names;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using  System.Web;

using System.Web.Script.Serialization;

    class LeaguesAndTeamsAsJson
    {
        static void Main()
        {
            var context = new FootballEntities();

            var leaguesAndTeams = context.Leagues
                .OrderBy(l => l.LeagueName)
                .Select(l => new
                {
                    leagueName = l.LeagueName,
                    teams = l.Teams
                        .OrderBy(t => t.TeamName)
                        .Select(t => t.TeamName)
                })
                .ToList();

            var jsSerializer = new JavaScriptSerializer();
            var output = jsSerializer.Serialize(leaguesAndTeams);
            File.WriteAllText("leagues-and-teams.json", output);
            Console.WriteLine("File leagues-and-teams.json was created in bin/Debug");
        }
    }
}
