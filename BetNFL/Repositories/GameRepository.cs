using Microsoft.Extensions.Configuration;
using BetNFL.Models;
using System.Collections.Generic;
using BetNFL.Utils;
using Microsoft.Data.SqlClient;

namespace BetNFL.Repositories
{
    public class GameRepository : BaseRepository, IGameRepository
    {
        public GameRepository(IConfiguration configuration) : base(configuration) { }

        public List<Game> GetAllGamesInWeek(int week)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT g.Id GameId, g.HomeTeamId, g.AwayTeamId, g.HomeTeamScore,
                               g.AwayTeamScore, g.KickoffTime, g.[Week], g.[Year],
                               ht.LocationName AS HomeLocationName, ht.TeamName AS HomeTeamName, 
                               ht.Abbreviation AS HomeAbbreviation, ht.LogoUrl AS HomeLogoUrl,
                               awt.LocationName AS AwayLocationName, awt.TeamName AS AwayTeamName, 
                               awt.Abbreviation AS AwayAbbreviation, awt.LogoUrl AS AwayLogoUrl
                        FROM Game g
                            LEFT JOIN Team ht ON ht.id = g.HomeTeamId
                            LEFT JOIN Team awt ON awt.id = g.AwayTeamId
                        WHERE g.Week = @week
                        ORDER BY g.KickoffTime ASC
                    ";
                    cmd.Parameters.AddWithValue("@week", week);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Game> games = new List<Game>();

                        while (reader.Read())
                        {
                            Game game = DbUtils.ReadGame(reader);
                            games.Add(game);
                        }

                        return games;
                    }
                }
            }
        }

        public List<Game> GetGamesWithOpenBets()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT g.Id GameId, g.HomeTeamId, g.AwayTeamId, g.HomeTeamScore,
                               g.AwayTeamScore, g.KickoffTime, g.[Week], g.[Year],
                               ht.LocationName AS HomeLocationName, ht.TeamName AS HomeTeamName, 
                               ht.Abbreviation AS HomeAbbreviation, ht.LogoUrl AS HomeLogoUrl,
                               awt.LocationName AS AwayLocationName, awt.TeamName AS AwayTeamName, 
                               awt.Abbreviation AS AwayAbbreviation, awt.LogoUrl AS AwayLogoUrl
                        FROM

                        /* table of unique GameId's that have open bets */
                            (
                                SELECT DISTINCT(GameId) 
                                FROM

                                    /* table of unique BetId's that have open bets */
                                    (
                                        SELECT DISTINCT(upb.BetId) OpenBetId
                                        FROM UserProfileBet upb
                                        WHERE ProcessedDateTime IS NULL
                                    ) openBet
                                    LEFT JOIN Bet b ON b.Id = OpenBet.OpenBetId
                            ) gameWithOpenBet
                            LEFT JOIN Game g ON g.Id = gameWithOpenBet.GameId
                            LEFT JOIN Team ht ON ht.id = g.HomeTeamId
                            LEFT JOIN Team awt ON awt.id = g.AwayTeamId
                        ORDER BY g.[Year] ASC, g.[Week]
                    ";

                    using (var reader = cmd.ExecuteReader())
                    {
                        var games = new List<Game>();

                        while (reader.Read())
                        {
                            Game game = DbUtils.ReadGame(reader);
                            games.Add(game);
                        }

                        return games;
                    }
                }
            }
        }

        public Game GetGameById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT g.Id GameId, g.HomeTeamId, g.AwayTeamId, g.HomeTeamScore,
                               g.AwayTeamScore, g.KickoffTime, g.[Week], g.[Year],
                               ht.LocationName AS HomeLocationName, ht.TeamName AS HomeTeamName, 
                               ht.Abbreviation AS HomeAbbreviation, ht.LogoUrl AS HomeLogoUrl,
                               awt.LocationName AS AwayLocationName, awt.TeamName AS AwayTeamName, 
                               awt.Abbreviation AS AwayAbbreviation, awt.LogoUrl AS AwayLogoUrl
                        FROM Game g
                            LEFT JOIN Team ht ON ht.id = g.HomeTeamId
                            LEFT JOIN Team awt ON awt.id = g.AwayTeamId
                        WHERE g.Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Game game = DbUtils.ReadGame(reader);
                            return game;
                        }

                        return null;
                    }
                }
            }
        }

        public Game GetGameWithLiveBets(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT g.Id GameId, g.HomeTeamId, g.AwayTeamId, g.HomeTeamScore,
                            g.AwayTeamScore, g.KickoffTime, g.[Week], g.[Year],
                            ht.LocationName HomeLocationName, ht.TeamName HomeTeamName, 
                            ht.Abbreviation HomeAbbreviation, ht.LogoUrl HomeLogoUrl,
                            awt.LocationName AwayLocationName, awt.TeamName AwayTeamName, 
                            awt.Abbreviation AwayAbbreviation, awt.LogoUrl AwayLogoUrl,
                            liveBet.Id AS BetId, liveBet.UserProfileId, liveBet.BetTypeId, liveBet.Line, 
                            liveBet.AwayTeamOdds, liveBet.HomeTeamOdds, liveBet.CreateDateTime, liveBet.isLive,
                            liveBet.Name, liveBet.Username
                        FROM Game g
                            LEFT JOIN Team ht ON ht.id = g.HomeTeamId
                            LEFT JOIN Team awt ON awt.id = g.AwayTeamId
                            LEFT JOIN 
                                (
                                    SELECT b.*, bt.Name, up.Username 
                                    FROM Bet b
                                        LEFT JOIN BetType bt ON bt.Id = b.BetTypeId
                                        LEFT JOIN UserProfile up ON up.Id = b.UserProfileId
                                    WHERE b.IsLive = 1
                                ) 
                                liveBet ON liveBet.GameId = g.Id
                            WHERE g.Id = @id
                            ORDER BY liveBet.Username ASC
                    ";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        Game game = null;

                        while (reader.Read())
                        {
                            if (game == null)
                            {
                                game = DbUtils.ReadGame(reader);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("BetId")))
                            {
                                var bet = DbUtils.ReadBet(reader);
                                game.LiveBets.Add(bet);
                            }
                        }

                        return game;
                    }
                }
            }
        }

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

        public void SetScore(Game game)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Game
                        SET AwayTeamScore = @awayTeamScore,
                            HomeTeamScore = @homeTeamScore
                        WHERE Id = @id
                    ";
                    DbUtils.InsertNullableInt(cmd, "@awayTeamScore", game.AwayTeamScore);
                    DbUtils.InsertNullableInt(cmd, "@homeTeamScore", game.HomeTeamScore);
                    cmd.Parameters.AddWithValue("@id", game.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteGame(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Game WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
