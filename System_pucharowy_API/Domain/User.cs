namespace System_pucharowy_API.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } = default!;
        public ICollection<TournamentParticipant> TournamentParticipants { get; set; } = new List<TournamentParticipant>();
        public ICollection<Match> MatchesAsPlayer1 { get; set; } = new List<Match>();
        public ICollection<Match> MatchesAsPlayer2 { get; set; } = new List<Match>();
        public ICollection<Match> MatchesAsWinner { get; set; } = new List<Match>();

    }
}
