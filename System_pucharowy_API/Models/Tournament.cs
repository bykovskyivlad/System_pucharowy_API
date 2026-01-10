namespace System_pucharowy_API.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = "Draft";

        public Bracket? Bracket { get; set; }
        public ICollection<TournamentParticipant> Participants { get; set; } = new List<TournamentParticipant>();
        public ICollection<Match> MatchesAsPlayer1 { get; set; } = new List<Match>();
        public ICollection<Match> MatchesAsPlayer2 { get; set; } = new List<Match>();
        public ICollection<Match> MatchesAsWinner { get; set; } = new List<Match>();
    }
}
