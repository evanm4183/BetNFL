using BetNFL.Models;
using BetNFL.Utils;
using Microsoft.Extensions.Configuration;

namespace BetNFL.Repositories
{
    public class BetRepository : BaseRepository, IBetRepository
    {
        public BetRepository(IConfiguration configuration) : base(configuration) { }

        public void PostBet(Bet bet)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE BET
                        SET IsLive = 0
                        WHERE GameId = @gameId
                            AND UserProfileId = @userProfileId
                            AND IsLive = 1

                        INSERT INTO Bet
                            (UserProfileId, GameId, BetTypeId, Line, AwayTeamOdds,
                             HomeTeamOdds, CreateDateTime, IsLive)
                        VALUES
                            (@userProfileId, @gameId, @betTypeId, @line, @awayTeamOdds,
                             @homeTeamOdds, GETDATE(), 1);
                    ";
                    cmd.Parameters.AddWithValue("@userProfileId", bet.UserProfileId);
                    cmd.Parameters.AddWithValue("@gameId", bet.GameId);
                    cmd.Parameters.AddWithValue("@betTypeId", bet.BetTypeId);
                    DbUtils.InsertNullableDouble(cmd, "@Line", bet.Line);
                    cmd.Parameters.AddWithValue("@awayTeamOdds", bet.AwayTeamOdds);
                    cmd.Parameters.AddWithValue("@homeTeamOdds", bet.HomeTeamOdds);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
