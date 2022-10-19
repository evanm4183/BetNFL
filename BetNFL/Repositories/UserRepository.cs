using Microsoft.Extensions.Configuration;
using BetNFL.Models;
using BetNFL.Utils;

namespace BetNFL.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public User GetByFirebaseUserId(string firebaseUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT u.Id, u.UserTypeId, u.Email,u.Username,
                               u.FirebaseUserId, u.AvailableFunds,
                               u.IsApproved, ut.Name
                        FROM [User] u
                            LEFT JOIN UserType ut ON ut.Id = u.UserTypeId
                        WHERE u.FirebaseUserId = @firebaseUserId
                    ";
                    cmd.Parameters.AddWithValue("@firebaseUserId", firebaseUserId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new User()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Email = DbUtils.GetString(reader, "Email"),
                                Username = DbUtils.GetString(reader, "Username"),
                                FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                                AvailableFunds = DbUtils.GetDecimal(reader, "AvailableFunds"),
                                IsApproved = DbUtils.GetBoolean(reader, "IsApproved"),
                                UserType = new UserType()
                                {
                                    Id = DbUtils.GetInt(reader, "UserTypeId"),
                                    Name = DbUtils.GetString(reader, "Name")
                                }
                            };
                            return user;
                        }

                        return null;
                    }
                }
            }
        }
    }
}
