﻿namespace BetNFL.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
