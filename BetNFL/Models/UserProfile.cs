namespace BetNFL.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public UserType UserType { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirebaseUserId { get; set; }
        public decimal AvailableFunds { get; set; }
        public bool IsApproved { get; set; }
    }
}
