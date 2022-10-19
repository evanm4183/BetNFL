using Microsoft.Data.SqlClient;
using System.Data.Common;

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

        public static decimal GetDecimal(SqlDataReader reader, string column)
        {
            return reader.GetDecimal(reader.GetOrdinal(column));
        }

        public static bool GetBoolean(SqlDataReader reader, string column)
        {
            return (reader.GetBoolean(reader.GetOrdinal(column)));
        }
    }
}
