using Microsoft.Extensions.Configuration;
using BetNFL.Models;
using BetNFL.Utils;

namespace BetNFL.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id, up.UserTypeId, up.Email, up.Username,
                               up.FirebaseUserId, up.AvailableFunds,
                               up.IsApproved, ut.Name
                        FROM UserProfile up
                            LEFT JOIN UserType ut ON ut.Id = up.UserTypeId
                        WHERE up.FirebaseUserId = @firebaseUserId
                    ";
                    cmd.Parameters.AddWithValue("@firebaseUserId", firebaseUserId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new UserProfile()
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
                                },
                                UserTypeId = DbUtils.GetInt(reader, "UserTypeId")
                            };
                            return user;
                        }

                        return null;
                    }
                }
            }
        }

        public void AddFunds(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                        SET AvailableFunds = @availableFunds
                        WHERE Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", userProfile.Id);
                    cmd.Parameters.AddWithValue("@availableFunds", userProfile.AvailableFunds);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RegisterNewUser(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile
                        (
                            UserTypeId, Email, Username, FirebaseUserId,
                            AvailableFunds, IsApproved
                        )
                        VALUES
                        (
                            @userTypeId, @email, @username, 
                            @firebaseUserId, 0, 1
                        )
                    ";
                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserType.Id);
                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@username", userProfile.Username);
                    cmd.Parameters.AddWithValue("@firebaseUserId", userProfile.FirebaseUserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
