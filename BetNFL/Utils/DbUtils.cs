using BetNFL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace BetNFL.Utils
{
    public static class DbUtils
    {
        public static string GetString(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetString(ordinal);
        }

        public static int GetInt(SqlDataReader reader, string column)
        {
            return reader.GetInt32(reader.GetOrdinal(column));
        }

        public static int? GetNullableInt(SqlDataReader reader, string column)
        {
            var ordinal = reader.GetOrdinal(column);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetInt32(ordinal);
        }

        public static decimal GetDecimal(SqlDataReader reader, string column)
        {
            return reader.GetDecimal(reader.GetOrdinal(column));
        }

        public static bool GetBoolean(SqlDataReader reader, string column)
        {
            return reader.GetBoolean(reader.GetOrdinal(column));
        }

        public static DateTime GetDateTime(SqlDataReader reader, string column)
        {
            return reader.GetDateTime(reader.GetOrdinal(column));
        }

        public static void InsertNullableInt(SqlCommand cmd, string column, int? value)
        {
            if (value == null)
            {
                cmd.Parameters.AddWithValue(column, DBNull.Value);
                return;
            }

            cmd.Parameters.AddWithValue(column, value);
            return;
        }

        public static void InsertNullableDouble(SqlCommand cmd, string column, double? value)
        {
            if (value == null)
            {
                cmd.Parameters.AddWithValue(column, DBNull.Value);
                return;
            }

            cmd.Parameters.AddWithValue(column, value);
            return;
        }


        // Must alias Game.Id as "GameId"
        public static Game ReadGame(SqlDataReader reader)
        {
            Game game = new Game()
            {
                Id = GetInt(reader, "GameId"),
                AwayTeamId = GetInt(reader, "AwayTeamId"),
                HomeTeamId = GetInt(reader, "HomeTeamId"),
                AwayTeamScore = GetNullableInt(reader, "AwayTeamScore"),
                HomeTeamScore = GetNullableInt(reader, "HomeTeamScore"),
                KickoffTime = GetDateTime(reader, "KickoffTime"),
                Week = GetInt(reader, "Week"),
                Year = GetInt(reader, "Year"),
                AwayTeam = new Team()
                {
                    Id = GetInt(reader, "AwayTeamId"),
                    LocationName = GetString(reader, "AwayLocationName"),
                    TeamName = GetString(reader, "AwayTeamName"),
                    Abbreviation = GetString(reader, "AwayAbbreviation"),
                    LogoUrl = GetString(reader, "AwayLogoUrl")
                },
                HomeTeam = new Team()
                {
                    Id = GetInt(reader, "HomeTeamId"),
                    LocationName = GetString(reader, "HomeLocationName"),
                    TeamName = GetString(reader, "HomeTeamName"),
                    Abbreviation = GetString(reader, "HomeAbbreviation"),
                    LogoUrl = GetString(reader, "HomeLogoUrl")
                },
                LiveBets = new List<Bet>()
            };

            return game;
        }

        // Must alias Bet.Id as "BetId"
        public static Bet ReadBet(SqlDataReader reader)
        {
            var bet = new Bet()
            {
                Id = GetInt(reader, "BetId"),
                UserProfileId = GetInt(reader, "UserProfileId"),
                GameId = GetInt(reader, "GameId"),
                Line = GetNullableInt(reader, "Line"),
                AwayTeamOdds = GetInt(reader, "AwayTeamOdds"),
                HomeTeamOdds = GetInt(reader, "HomeTeamOdds"),
                CreateDateTime = GetDateTime(reader, "CreateDateTime"),
                isLive = GetBoolean(reader, "IsLive"),
                BetType = new BetType()
                {
                    Id = GetInt(reader, "BetTypeId"),
                    Name = GetString(reader, "Name")
                },
                UserProfile = new UserProfile()
                {
                    Username = GetString(reader, "Username")
                },
                Game = new Game()
            };

            return bet;
        }

        // must alias UserProfileBet.Id as "UserProfileBetId" and UserProfileBet.UserProfileId as "BettorId"
        public static UserProfileBet ReadUserProfileBet(SqlDataReader reader)
        {
            var upb = new UserProfileBet()
            {
                Id = DbUtils.GetInt(reader, "UserProfileBetId"),
                UserProfileId = DbUtils.GetInt(reader, "BettorId"),
                BetId = DbUtils.GetInt(reader, "BetId"),
                WinnerId = DbUtils.GetNullableInt(reader, "WinnerId"),
                Side = DbUtils.GetInt(reader, "Side"),
                BetAmount = DbUtils.GetDecimal(reader, "BetAmount"),
                CreateDateTime = DbUtils.GetDateTime(reader, "upbCreateDateTime"),
                ProcessedDateTime = DbUtils.GetDateTime(reader, "ProcessedDateTime"),
                Bet = DbUtils.ReadBet(reader)
            };
            upb.Bet.Game = DbUtils.ReadGame(reader);

            return upb;
        }
    }
}
