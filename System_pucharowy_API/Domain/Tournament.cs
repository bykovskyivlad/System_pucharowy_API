namespace System_pucharowy_API.Domain
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public string Status { get; set; } = "Draft";

        public Bracket? Bracket { get; set; }
        public ICollection<TournamentParticipant> Participants { get; set; } = new List<TournamentParticipant>();
       
    }
}
