using BetNFL.Models;
using BetNFL.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace BetNFL.Repositories
{
    public class UserProfileBetRepository : BaseRepository, IUserProfileBetRepository
    {
        public UserProfileBetRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfileBet> GetMyOpenBets(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT upb.Id UserProfileBetId, upb.UserProfileId BettorId, upb.BetId,
                               upb.WinnerId, upb.Side, upb.BetAmount, upb.CreateDateTime upbCreateDateTime,
                               b.Id BetId, b.UserProfileId, b.BetTypeId, b.Line, 
                               b.AwayTeamOdds, b.HomeTeamOdds, b.CreateDateTime, b.isLive,
                               g.Id GameId, g.HomeTeamId, g.AwayTeamId, g.HomeTeamScore,
                               g.AwayTeamScore, g.KickoffTime, g.[Week], g.[Year],
                               awt.LocationName AwayLocationName, awt.TeamName AwayTeamName, 
                               awt.Abbreviation AwayAbbreviation, awt.LogoUrl AwayLogoUrl,
                               ht.LocationName HomeLocationName, ht.TeamName HomeTeamName, 
                               ht.Abbreviation HomeAbbreviation, ht.LogoUrl HomeLogoUrl,
                               sb.Username, bt.Name
                        FROM UserProfileBet upb
                            LEFT JOIN Bet b ON b.Id = upb.BetId
                            LEFT JOIN Game g ON g.Id = b.GameId
                            LEFT JOIN Team ht ON ht.id = g.HomeTeamId
                            LEFT JOIN Team awt ON awt.id = g.AwayTeamId
                            LEFT JOIN UserProfile sb ON sb.Id = b.UserProfileId
                            LEFT JOIN BetType bt ON bt.Id = b.BetTypeId
                        WHERE upb.UserProfileId = @userId AND upb.ProcessedDateTime IS NULL
                        ORDER BY upb.CreateDateTime DESC
                    ";
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<UserProfileBet> openBets = new List<UserProfileBet>();

                        while (reader.Read())
                        {
                            UserProfileBet openBet = new UserProfileBet()
                            {
                                Id = DbUtils.GetInt(reader, "UserProfileBetId"),
                                UserProfileId = DbUtils.GetInt(reader, "BettorId"),
                                BetId = DbUtils.GetInt(reader, "BetId"),
                                WinnerId = DbUtils.GetNullableInt(reader, "WinnerId"),
                                Side = DbUtils.GetInt(reader, "Side"),
                                BetAmount = DbUtils.GetDecimal(reader, "BetAmount"),
                                CreateDateTime = DbUtils.GetDateTime(reader, "upbCreateDateTime"),
                                Bet = DbUtils.ReadBet(reader)
                            };
                            openBet.Bet.Game = DbUtils.ReadGame(reader);

                            openBets.Add(openBet);
                        }

                        return openBets;
                    }
                }
            }
        }

        public void PostUserProfileBet(UserProfileBet upBet)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfileBet
                        (
                            UserProfileId, BetId, WinnerId, Side,
                            BetAmount, CreateDateTime, ProcessedDateTime
                        )
                        VALUES
                        (
                            @userProfileId, @betId, NULL, @side, 
                            @betAmount, GETDATE(), NULL
                        );

                        UPDATE UserProfile 
                        SET AvailableFunds = AvailableFunds - @betAmount
                        WHERE Id = @userProfileId
                    ";
                    cmd.Parameters.AddWithValue("@userProfileId", upBet.UserProfileId);
                    cmd.Parameters.AddWithValue("@betId", upBet.BetId);
                    cmd.Parameters.AddWithValue("@side", upBet.Side);
                    cmd.Parameters.AddWithValue("@betAmount", upBet.BetAmount);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
