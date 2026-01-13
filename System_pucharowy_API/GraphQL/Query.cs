using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using System_pucharowy_API.Data;
using System_pucharowy_API.Domain;

namespace System_pucharowy_API.GraphQL;

public class Query
{
    [Authorize]
    public async Task<User> Me(AppDbContext db, IHttpContextAccessor http)
    {
        var principal = http.HttpContext?.User;
        var sub = principal?.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(sub))
            throw new GraphQLException("Not authenticated.");

        var userId = int.Parse(sub);
        return await db.Users.AsNoTracking().FirstAsync(u => u.Id == userId);
    }

    [Authorize]
    public async Task<List<Match>> MyMatches(AppDbContext db, IHttpContextAccessor http)
    {
        var principal = http.HttpContext?.User;
        var sub = principal?.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(sub))
            throw new GraphQLException("Not authenticated.");

        var userId = int.Parse(sub);

        return await db.Matches
            .AsNoTracking()
            .Include(m => m.Player1)
            .Include(m => m.Player2)
            .Include(m => m.Winner)
            .Where(m => m.Player1Id == userId || m.Player2Id == userId)
            .OrderBy(m => m.Round)
            .ThenBy(m => m.PositionInRound)
            .ToListAsync();
    }
}
