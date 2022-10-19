﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BetNFL.Models;
using BetNFL.Utils;

namespace BetNFL.Repositories
{
    public class SiteTimeRepository : BaseRepository, ISiteTimeRepository
    {
        public SiteTimeRepository(IConfiguration configuration) : base(configuration) { }

        public SiteTime GetSiteTime()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM SiteTime";

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var currTime = new SiteTime()
                            {
                                CurrentYear = DbUtils.GetInt(reader, "CurrentYear"),
                                CurrentWeek = DbUtils.GetInt(reader, "CurrentWeek")
                            };
                            return currTime;
                        }

                        return null;
                    }
                }
            }
        }
    }
}
