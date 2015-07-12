using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List_Team_Names
{
    class ListTeamNames
    {
        static void Main()
        {
            var context = new FootballEntities();
            foreach (var teamName in context.Teams)
            {
                Console.WriteLine(teamName.TeamName);
            }
        }
    }
}
