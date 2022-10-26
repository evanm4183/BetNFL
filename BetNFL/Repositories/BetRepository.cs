using BetNFL.Models;
using BetNFL.Utils;
using Microsoft.Extensions.Configuration;

namespace BetNFL.Repositories
{
    public class BetRepository : BaseRepository, IBetRepository
    {
        public BetRepository(IConfiguration configuration) : base(configuration) { }

        public Bet GetLiveBetForGame(int userProfileId, int gameId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT * FROM Bet
                        WHERE UserProfileId = @userProfileId
                            AND GameId = @gameId
                            AND IsLive = 1
                    ";
                    cmd.Parameters.AddWithValue("@userProfileId", userProfileId);
                    cmd.Parameters.AddWithValue("@gameId", gameId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var bet = new Bet()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                                GameId = DbUtils.GetInt(reader, "GameId"),
                                Line = DbUtils.GetNullableInt(reader, "Line"),
                                AwayTeamOdds = DbUtils.GetInt(reader, "AwayTeamOdds"),
                                HomeTeamOdds = DbUtils.GetInt(reader, "HomeTeamOdds"),
                                CreateDateTime = DbUtils.GetDateTime(reader, "CreateDateTime"),
                                isLive = DbUtils.GetBoolean(reader, "IsLive")
                            };
                            return bet;
                        }

                        return null;
                    }
                }
            }
        }

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
