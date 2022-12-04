using BetNFL.Models;
using BetNFL.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace BetNFL.Repositories
{
    public class UserProfileBetRepository : BaseRepository, IUserProfileBetRepository
    {
        public UserProfileBetRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfileBet> GetBettorOpenBets(int userId)
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
                        WHERE upb.UserProfileId = @userId 
                            AND upb.ProcessedDateTime IS NULL
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

        public List<UserProfileBet> GetSportsbookOpenBets(int userId)
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
                               sb.Username, bt.Name, btr.Username BettorUsername
                        FROM UserProfileBet upb
                            LEFT JOIN Bet b ON b.Id = upb.BetId
                            LEFT JOIN Game g ON g.Id = b.GameId
                            LEFT JOIN Team ht ON ht.id = g.HomeTeamId
                            LEFT JOIN Team awt ON awt.id = g.AwayTeamId
                            LEFT JOIN UserProfile sb ON sb.Id = b.UserProfileId
                            LEFT JOIN UserProfile btr ON btr.Id = upb.UserProfileId
                            LEFT JOIN BetType bt ON bt.Id = b.BetTypeId
                        WHERE b.UserProfileId = @userId 
                            AND upb.ProcessedDateTime IS NULL
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
                            openBet.UserProfile = new UserProfile()
                            {
                                Username = DbUtils.GetString(reader, "BettorUsername")
                            };

                            openBets.Add(openBet);
                        }

                        return openBets;
                    }
                }
            }
        }

        public List<UserProfileBet> GetBettorSettledBets(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT upb.Id UserProfileBetId, upb.UserProfileId BettorId, upb.BetId,
                               upb.WinnerId, upb.Side, upb.BetAmount, 
                               upb.CreateDateTime upbCreateDateTime,
                               upb.ProcessedDateTime,
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
                        WHERE upb.UserProfileId = @userId
                            AND upb.ProcessedDateTime IS NOT NULL
                        ORDER BY upb.ProcessedDateTime DESC
                    ";
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<UserProfileBet> settledBets = new List<UserProfileBet>();

                        while (reader.Read())
                        {
                            var settledBet = DbUtils.ReadUserProfileBet(reader);

                            settledBets.Add(settledBet);
                        }

                        return settledBets;
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
                        WHERE Id = @userProfileId;
                    ";
                    cmd.Parameters.AddWithValue("@userProfileId", upBet.UserProfileId);
                    cmd.Parameters.AddWithValue("@betId", upBet.BetId);
                    cmd.Parameters.AddWithValue("@side", upBet.Side);
                    cmd.Parameters.AddWithValue("@betAmount", upBet.BetAmount);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SettleOpenBetsByGame(int gameId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    int awayTeamScore;
                    int homeTeamScore;

                    cmd.CommandText = @"
                        SELECT AwayTeamScore, HomeTeamScore
                        FROM Game
                        WHERE Id = @gameId
                    ";
                    cmd.Parameters.AddWithValue(@"gameId", gameId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        awayTeamScore = DbUtils.GetInt(reader, "AwayTeamScore");
                        homeTeamScore = DbUtils.GetInt(reader, "HomeTeamScore");
                    }
                        
                    if (awayTeamScore > homeTeamScore)
                    {
                        cmd.CommandText = @"
                            /* Add winnings to the accounts of the bettors that won */

                            UPDATE up
							SET up.AvailableFunds = up.AvailableFunds + upEarnings.Winnings
							FROM UserProfile up
								JOIN 
								(
									SELECT 
										upb.UserProfileId,
										SUM(
											CASE
												WHEN AwayTeamOdds > 0 THEN BetAmount + (BetAmount * (AwayTeamOdds / 100.0))
												WHEN AwayTeamOdds < 0 THEN BetAmount + (BetAmount * (100.0 / -AwayTeamOdds))
											END
										) AS Winnings
									FROM UserProfileBet upb
										LEFT JOIN Bet b ON b.Id = upb.BetId
									WHERE b.GameId = @gameId 
                                        AND Side = 1 
                                        AND upb.ProcessedDateTime IS NULL
									GROUP BY upb.UserProfileId
								) upEarnings ON upEarnings.UserProfileId = up.Id;


							/* Subtract losses from the accounts of the sportsbooks that lost */

                            UPDATE up
							SET up.AvailableFunds = up.AvailableFunds - upEarnings.Losses
							FROM UserProfile up
								JOIN 
								(
									SELECT 
										b.UserProfileId,
										SUM(
											CASE
												WHEN AwayTeamOdds > 0 THEN (BetAmount * (AwayTeamOdds / 100.0))
												WHEN AwayTeamOdds < 0 THEN (BetAmount * (100.0 / -AwayTeamOdds))
											END
										) AS Losses
									FROM UserProfileBet upb
										LEFT JOIN Bet b ON b.Id = upb.BetId
									WHERE b.GameId = @gameId 
                                        AND Side = 1 
                                        AND upb.ProcessedDateTime IS NULL
									GROUP BY b.UserProfileId
								) upEarnings ON upEarnings.UserProfileId = up.Id;


                            /* Add winnings to the sportsbooks that won */

                            UPDATE up
							SET up.AvailableFunds = up.AvailableFunds + upEarnings.Winnings
							FROM UserProfile up
								JOIN 
								(
									SELECT 
										b.UserProfileId,
										SUM(BetAmount) AS Winnings
									FROM UserProfileBet upb
										LEFT JOIN Bet b ON b.Id = upb.BetId
									WHERE b.GameId = @gameId 
                                        AND Side = 2 
                                        AND upb.ProcessedDateTime IS NULL
									GROUP BY b.UserProfileId
								) upEarnings ON upEarnings.UserProfileId = up.Id;


                            /* Marks all open bets as processed */

                            UPDATE upb
                            SET upb.ProcessedDateTime = GETDATE()
                            FROM UserProfileBet upb
	                            LEFT JOIN Bet b ON b.Id = upb.BetId
                            WHERE GameId = @gameId
	                            AND upb.ProcessedDateTime IS NULL
                        ";

                        cmd.ExecuteNonQuery();
                    }
                    else if (awayTeamScore < homeTeamScore)
                    {
                        cmd.CommandText = @"
							/* Add winnings to the accounts of the bettors that won */

                            UPDATE up
							SET up.AvailableFunds = up.AvailableFunds + upEarnings.Winnings
							FROM UserProfile up
								JOIN 
								(
									SELECT 
										upb.UserProfileId,
										SUM(
											CASE
												WHEN HomeTeamOdds > 0 THEN BetAmount + (BetAmount * (HomeTeamOdds / 100.0))
												WHEN HomeTeamOdds < 0 THEN BetAmount + (BetAmount * (100.0 / -HomeTeamOdds))
											END
										) AS Winnings
									FROM UserProfileBet upb
										LEFT JOIN Bet b ON b.Id = upb.BetId
									WHERE b.GameId = @gameId 
                                        AND Side = 2 
                                        AND upb.ProcessedDateTime IS NULL
									GROUP BY upb.UserProfileId
								) upEarnings ON upEarnings.UserProfileId = up.Id;


							/* Subtract losses from the accounts of the sportsbooks that lost */

                            UPDATE up
							SET up.AvailableFunds = up.AvailableFunds - upEarnings.Losses
							FROM UserProfile up
								JOIN 
								(
									SELECT 
										b.UserProfileId,
										SUM(
											CASE
												WHEN HomeTeamOdds > 0 THEN (BetAmount * (HomeTeamOdds / 100.0))
												WHEN HomeTeamOdds < 0 THEN (BetAmount * (100.0 / -HomeTeamOdds))
											END
										) AS Losses
									FROM UserProfileBet upb
										LEFT JOIN Bet b ON b.Id = upb.BetId
									WHERE b.GameId = @gameId 
                                        AND Side = 2 
                                        AND upb.ProcessedDateTime IS NULL
									GROUP BY b.UserProfileId
								) upEarnings ON upEarnings.UserProfileId = up.Id;


                            /* Add winnings to the sportsbooks that won */

                            UPDATE up
							SET up.AvailableFunds = up.AvailableFunds + upEarnings.Winnings
							FROM UserProfile up
								JOIN 
								(
									SELECT 
										b.UserProfileId,
										SUM(BetAmount) AS Winnings
									FROM UserProfileBet upb
										LEFT JOIN Bet b ON b.Id = upb.BetId
									WHERE b.GameId = @gameId 
                                        AND Side = 1 
                                        AND upb.ProcessedDateTime IS NULL
									GROUP BY b.UserProfileId
								) upEarnings ON upEarnings.UserProfileId = up.Id;


                            /* Marks all open bets as processed */

                            UPDATE upb
                            SET upb.ProcessedDateTime = GETDATE()
                            FROM UserProfileBet upb
	                            LEFT JOIN Bet b ON b.Id = upb.BetId
                            WHERE GameId = @gameId
	                            AND upb.ProcessedDateTime IS NULL
                        ";

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.CommandText = @"
                            UPDATE up
							SET up.AvailableFunds = upEarnings.Refund
							FROM UserProfile up
								JOIN 
								(
									SELECT upb.UserProfileId, SUM(BetAmount) AS Refund
									FROM UserProfileBet upb
										LEFT JOIN Bet b ON b.Id = upb.BetId
									WHERE b.GameId = @gameId
									GROUP BY upb.UserProfileId
								) upEarnings ON upEarnings.UserProfileId = up.Id

                
                            /* Marks all open bets as processed */

                            UPDATE upb
                            SET upb.ProcessedDateTime = GETDATE()
                            FROM UserProfileBet upb
	                            LEFT JOIN Bet b ON b.Id = upb.BetId
                            WHERE GameId = @gameId
	                            AND upb.ProcessedDateTime IS NULL
                        ";

						cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
