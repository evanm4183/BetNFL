using System.Collections.Generic;
using BetNFL.Models;
using BetNFL.Utils;
using Microsoft.Extensions.Configuration;

namespace BetNFL.Repositories
{
    public class UserTypeRepository : BaseRepository, IUserTypeRepository
    {
        public UserTypeRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserType> GetPublicUserTypes()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT * FROM UserType
                        WHERE Name = 'bettor' OR Name = 'sportsbook'
                    ";

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<UserType> userTypes = new List<UserType>();

                        while (reader.Read())
                        {
                            UserType userType = new UserType()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                            };
                            userTypes.Add(userType);
                        }

                        return userTypes;
                    }
                }
            }
        }
    }
}
