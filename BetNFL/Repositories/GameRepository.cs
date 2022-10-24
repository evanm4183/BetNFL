﻿using Microsoft.Extensions.Configuration;
using BetNFL.Models;
using System.Collections.Generic;
using BetNFL.Utils;

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
                        SELECT g.Id, g.HomeTeamId, g.AwayTeamId, g.HomeTeamScore,
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
                            Game game = new Game()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                AwayTeamId = DbUtils.GetInt(reader, "AwayTeamId"),
                                HomeTeamId = DbUtils.GetInt(reader, "HomeTeamId"),
                                AwayTeamScore = DbUtils.GetNullableInt(reader, "AwayTeamScore"),
                                HomeTeamScore = DbUtils.GetNullableInt(reader, "HomeTeamScore"),
                                KickoffTime = DbUtils.GetDateTime(reader, "KickoffTime"),
                                Week = DbUtils.GetInt(reader, "Week"),
                                Year = DbUtils.GetInt(reader, "Year"),
                                AwayTeam = new Team()
                                {
                                    Id = DbUtils.GetInt(reader, "AwayTeamId"),
                                    LocationName = DbUtils.GetString(reader, "AwayLocationName"),
                                    TeamName = DbUtils.GetString(reader, "AwayTeamName"),
                                    Abbreviation = DbUtils.GetString(reader, "AwayAbbreviation"),
                                    LogoUrl = DbUtils.GetString(reader, "AwayLogoUrl")
                                },
                                HomeTeam = new Team()
                                {
                                    Id = DbUtils.GetInt(reader, "HomeTeamId"),
                                    LocationName = DbUtils.GetString(reader, "HomeLocationName"),
                                    TeamName = DbUtils.GetString(reader, "HomeTeamName"),
                                    Abbreviation = DbUtils.GetString(reader, "HomeAbbreviation"),
                                    LogoUrl = DbUtils.GetString(reader, "HomeLogoUrl")
                                }
                            };
                            games.Add(game);
                        }

                        return games;
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
    }
}
