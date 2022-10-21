using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using BetNFL.Models;
using BetNFL.Utils;

namespace BetNFL.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        public TeamRepository(IConfiguration configuration) : base(configuration) { }
        public List<Team> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT * FROM Team
                        ORDER BY LocationName ASC, TeamName ASC
                    ";

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Team> teams = new List<Team>();

                        while (reader.Read())
                        {
                            Team team = new Team()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                LocationName = DbUtils.GetString(reader, "LocationName"),
                                TeamName = DbUtils.GetString(reader, "TeamName"),
                                Abbreviation = DbUtils.GetString(reader, "Abbreviation"),
                                LogoUrl = DbUtils.GetString(reader, "LogoUrl")
                            };
                            teams.Add(team);
                        }

                        return teams;
                    }
                }
            }
        }
    }
}
