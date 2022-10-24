using Microsoft.Extensions.Configuration;
using BetNFL.Models;

namespace BetNFL.Repositories
{
    public class GameRepository : BaseRepository, IGameRepository
    {
        public GameRepository(IConfiguration configuration) : base(configuration) { }

        public void PostGame(Game game)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Game
                            (HomeTeamId, AwayTeamId, HomeTeamScore, AwayTeamScore, KickoffTime, Week, Year)
                        VALUES
                            (@homeTeamId, @awayTeamId, NULL, NULL, @kickoffTime, @week, @year)
                    ";
                    cmd.Parameters.AddWithValue("@homeTeamId", game.HomeTeamId);
                    cmd.Parameters.AddWithValue("@awayTeamId", game.AwayTeamId);
                    cmd.Parameters.AddWithValue("@kickoffTime", game.KickoffTime);
                    cmd.Parameters.AddWithValue("@week", game.Week);
                    cmd.Parameters.AddWithValue("@year", game.Year);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
