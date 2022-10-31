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
                            UPDATE allInfo
                            SET 
	                            BettorAvailableFunds = 
	                                CASE
		                                WHEN AwayTeamOdds > 0 THEN BettorAvailableFunds + BetAmount + (BetAmount * (AwayTeamOdds / 100.0))
		                                WHEN AwayTeamOdds < 0 THEN BettorAvailableFunds + BetAmount + (BetAmount * (100.0 / -AwayTeamOdds))
	                                END
                            FROM
	                            (
		                            SELECT 
			                            upb.Id AS UserProfileBetId,
			                            upb.Side,
			                            upb.BetAmount,
			                            b.AwayTeamOdds,
			                            b.HomeTeamOdds,
			                            g.Id AS GameId,
			                            g.AwayTeamScore,
			                            g.HomeTeamScore,
			                            bp.Id AS BettorId,
			                            bp.Username AS BettorName,
			                            bp.AvailableFunds AS BettorAvailableFunds,
			                            sbp.Id AS SportsbookId,
			                            sbp.Username AS SportsbookName,
			                            sbp.AvailableFunds AS SportsbookAvailableFunds,
			                            upb.WinnerId,
			                            upb.ProcessedDateTime
		                            FROM UserProfileBet upb
			                            LEFT JOIN Bet b ON b.Id = upb.BetId
			                            LEFT JOIN Game g ON g.Id = b.GameId
			                            LEFT JOIN UserProfile bp ON bp.Id = upb.UserProfileId
			                            LEFT JOIN UserProfile sbp ON sbp.Id = b.UserProfileId
		                            WHERE g.Id = @gameId AND upb.ProcessedDateTime IS NULL				
	                            ) allInfo
                            WHERE Side = 1;


                            UPDATE allInfo
                            SET 
	                            SportsbookAvailableFunds = 
	                                CASE
		                                WHEN AwayTeamOdds > 0 THEN SportsbookAvailableFunds - (BetAmount * (AwayTeamOdds / 100.0))
		                                WHEN AwayTeamOdds < 0 THEN SportsbookAvailableFunds - (BetAmount * (100.0 / -AwayTeamOdds))
	                                END
                            FROM
	                            (
		                            SELECT 
			                            upb.Id AS UserProfileBetId,
			                            upb.Side,
			                            upb.BetAmount,
			                            b.AwayTeamOdds,
			                            b.HomeTeamOdds,
			                            g.Id AS GameId,
			                            g.AwayTeamScore,
			                            g.HomeTeamScore,
			                            bp.Id AS BettorId,
			                            bp.Username AS BettorName,
			                            bp.AvailableFunds AS BettorAvailableFunds,
			                            sbp.Id AS SportsbookId,
			                            sbp.Username AS SportsbookName,
			                            sbp.AvailableFunds AS SportsbookAvailableFunds,
			                            upb.WinnerId,
			                            upb.ProcessedDateTime
		                            FROM UserProfileBet upb
			                            LEFT JOIN Bet b ON b.Id = upb.BetId
			                            LEFT JOIN Game g ON g.Id = b.GameId
			                            LEFT JOIN UserProfile bp ON bp.Id = upb.UserProfileId
			                            LEFT JOIN UserProfile sbp ON sbp.Id = b.UserProfileId
		                            WHERE g.Id = @gameId AND upb.ProcessedDateTime IS NULL				
	                            ) allInfo
                            WHERE Side = 1;


                            UPDATE allInfo
                            SET 
	                            SportsbookAvailableFunds = SportsbookAvailableFunds + BetAmount
                            FROM
	                            (
		                            SELECT 
			                            upb.Id AS UserProfileBetId,
			                            upb.Side,
			                            upb.BetAmount,
			                            b.AwayTeamOdds,
			                            b.HomeTeamOdds,
			                            g.Id AS GameId,
			                            g.AwayTeamScore,
			                            g.HomeTeamScore,
			                            bp.Id AS BettorId,
			                            bp.Username AS BettorName,
			                            bp.AvailableFunds AS BettorAvailableFunds,
			                            sbp.Id AS SportsbookId,
			                            sbp.Username AS SportsbookName,
			                            sbp.AvailableFunds AS SportsbookAvailableFunds,
			                            upb.WinnerId,
			                            upb.ProcessedDateTime
		                            FROM UserProfileBet upb
			                            LEFT JOIN Bet b ON b.Id = upb.BetId
			                            LEFT JOIN Game g ON g.Id = b.GameId
			                            LEFT JOIN UserProfile bp ON bp.Id = upb.UserProfileId
			                            LEFT JOIN UserProfile sbp ON sbp.Id = b.UserProfileId
		                            WHERE g.Id = @gameId AND upb.ProcessedDateTime IS NULL				
	                            ) allInfo
                            WHERE Side = 2;
                        ";

                        cmd.ExecuteNonQuery();
                    }
                    else if (awayTeamScore < homeTeamScore)
                    {
                        cmd.CommandText = @"
                            UPDATE allInfo
                            SET 
	                            BettorAvailableFunds = 
	                                CASE
		                                WHEN HomeTeamOdds > 0 THEN BettorAvailableFunds + BetAmount + (BetAmount * (HomeTeamOdds / 100.0))
		                                WHEN HomeTeamOdds < 0 THEN BettorAvailableFunds + BetAmount + (BetAmount * (100.0 / -HomeTeamOdds))
	                                END
                            FROM
	                            (
		                            SELECT 
			                            upb.Id AS UserProfileBetId,
			                            upb.Side,
			                            upb.BetAmount,
			                            b.AwayTeamOdds,
			                            b.HomeTeamOdds,
			                            g.Id AS GameId,
			                            g.AwayTeamScore,
			                            g.HomeTeamScore,
			                            bp.Id AS BettorId,
			                            bp.Username AS BettorName,
			                            bp.AvailableFunds AS BettorAvailableFunds,
			                            sbp.Id AS SportsbookId,
			                            sbp.Username AS SportsbookName,
			                            sbp.AvailableFunds AS SportsbookAvailableFunds,
			                            upb.WinnerId,
			                            upb.ProcessedDateTime
		                            FROM UserProfileBet upb
			                            LEFT JOIN Bet b ON b.Id = upb.BetId
			                            LEFT JOIN Game g ON g.Id = b.GameId
			                            LEFT JOIN UserProfile bp ON bp.Id = upb.UserProfileId
			                            LEFT JOIN UserProfile sbp ON sbp.Id = b.UserProfileId
		                            WHERE g.Id = @gameId AND upb.ProcessedDateTime IS NULL AND Side = 2			
	                            ) allInfo



							UPDATE allInfo
                            SET 
	                            SportsbookAvailableFunds = 
	                                CASE
		                                WHEN HomeTeamOdds > 0 THEN SportsbookAvailableFunds - (BetAmount * (HomeTeamOdds / 100.0))
		                                WHEN HomeTeamOdds < 0 THEN SportsbookAvailableFunds - (BetAmount * (100.0 / -HomeTeamOdds))
	                                END
                            FROM
	                            (
		                            SELECT 
			                            upb.Id AS UserProfileBetId,
			                            upb.Side,
			                            upb.BetAmount,
			                            b.AwayTeamOdds,
			                            b.HomeTeamOdds,
			                            g.Id AS GameId,
			                            g.AwayTeamScore,
			                            g.HomeTeamScore,
			                            bp.Id AS BettorId,
			                            bp.Username AS BettorName,
			                            bp.AvailableFunds AS BettorAvailableFunds,
			                            sbp.Id AS SportsbookId,
			                            sbp.Username AS SportsbookName,
			                            sbp.AvailableFunds AS SportsbookAvailableFunds,
			                            upb.WinnerId,
			                            upb.ProcessedDateTime
		                            FROM UserProfileBet upb
			                            LEFT JOIN Bet b ON b.Id = upb.BetId
			                            LEFT JOIN Game g ON g.Id = b.GameId
			                            LEFT JOIN UserProfile bp ON bp.Id = upb.UserProfileId
			                            LEFT JOIN UserProfile sbp ON sbp.Id = b.UserProfileId
		                            WHERE g.Id = @gameId AND upb.ProcessedDateTime IS NULL				
	                            ) allInfo
                            WHERE Side = 2;


                            UPDATE allInfo
                            SET 
	                            SportsbookAvailableFunds = SportsbookAvailableFunds + BetAmount
                            FROM
	                            (
		                            SELECT 
			                            upb.Id AS UserProfileBetId,
			                            upb.Side,
			                            upb.BetAmount,
			                            b.AwayTeamOdds,
			                            b.HomeTeamOdds,
			                            g.Id AS GameId,
			                            g.AwayTeamScore,
			                            g.HomeTeamScore,
			                            bp.Id AS BettorId,
			                            bp.Username AS BettorName,
			                            bp.AvailableFunds AS BettorAvailableFunds,
			                            sbp.Id AS SportsbookId,
			                            sbp.Username AS SportsbookName,
			                            sbp.AvailableFunds AS SportsbookAvailableFunds,
			                            upb.WinnerId,
			                            upb.ProcessedDateTime
		                            FROM UserProfileBet upb
			                            LEFT JOIN Bet b ON b.Id = upb.BetId
			                            LEFT JOIN Game g ON g.Id = b.GameId
			                            LEFT JOIN UserProfile bp ON bp.Id = upb.UserProfileId
			                            LEFT JOIN UserProfile sbp ON sbp.Id = b.UserProfileId
		                            WHERE g.Id = @gameId AND upb.ProcessedDateTime IS NULL				
	                            ) allInfo
                            WHERE Side = 1
                        ";

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.CommandText = @"
                            UPDATE allInfo
                            SET 
	                            BettorAvailableFunds = BettorAvailableFunds + BetAmount
                            FROM
	                            (
		                            SELECT 
			                            upb.Id AS UserProfileBetId,
			                            upb.Side,
			                            upb.BetAmount,
			                            b.AwayTeamOdds,
			                            b.HomeTeamOdds,
			                            g.Id AS GameId,
			                            g.AwayTeamScore,
			                            g.HomeTeamScore,
			                            bp.Id AS BettorId,
			                            bp.Username AS BettorName,
			                            bp.AvailableFunds AS BettorAvailableFunds,
			                            sbp.Id AS SportsbookId,
			                            sbp.Username AS SportsbookName,
			                            sbp.AvailableFunds AS SportsbookAvailableFunds,
			                            upb.WinnerId,
			                            upb.ProcessedDateTime
		                            FROM UserProfileBet upb
			                            LEFT JOIN Bet b ON b.Id = upb.BetId
			                            LEFT JOIN Game g ON g.Id = b.GameId
			                            LEFT JOIN UserProfile bp ON bp.Id = upb.UserProfileId
			                            LEFT JOIN UserProfile sbp ON sbp.Id = b.UserProfileId
		                            WHERE g.Id = @gameId AND upb.ProcessedDateTime IS NULL				
	                            ) allInfo
							WHERE Side = 1 OR Side = 2
                        ";

						cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
