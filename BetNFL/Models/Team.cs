namespace BetNFL.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string TeamName { get; set; }
        public string Abbreviation { get; set; }
        public string LogoUrl { get; set; }
        public string FullName
        {
            get
            {
                return LocationName + " " + TeamName;
            }
        }
    }
}
