using Microsoft.EntityFrameworkCore;
using System_pucharowy_API.Domain;

namespace System_pucharowy_API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Tournament> Tournaments { get; set; } = default!;
    public DbSet<Bracket> Brackets { get; set; } = default!;
    public DbSet<Match> Matches { get; set; } = default!;
    public DbSet<TournamentParticipant> TournamentParticipants { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        
        modelBuilder.Entity<Tournament>()
            .HasOne(t => t.Bracket)
            .WithOne(b => b.Tournament)
            .HasForeignKey<Bracket>(b => b.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<TournamentParticipant>()
            .HasKey(tp => new { tp.TournamentId, tp.UserId });

        modelBuilder.Entity<TournamentParticipant>()
            .HasOne(tp => tp.Tournament)
            .WithMany(t => t.Participants)
            .HasForeignKey(tp => tp.TournamentId);

        modelBuilder.Entity<TournamentParticipant>()
            .HasOne(tp => tp.User)
            .WithMany(u => u.TournamentParticipants)
            .HasForeignKey(tp => tp.UserId);

        
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Player1)
            .WithMany(u => u.MatchesAsPlayer1)
            .HasForeignKey(m => m.Player1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.Player2)
            .WithMany(u => u.MatchesAsPlayer2)
            .HasForeignKey(m => m.Player2Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.Winner)
            .WithMany(u => u.MatchesAsWinner)
            .HasForeignKey(m => m.WinnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
