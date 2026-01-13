using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System_pucharowy_API.Authozycja;
using System_pucharowy_API.Data;
using System_pucharowy_API.Domain;

namespace System_pucharowy_API.GraphQL
{
    public record AuthPayload(string Token, User User);
    public class Mutation
    {
        public async Task<AuthPayload> Register(
        AppDbContext db,
        PasswordHasher hasher,
        JwtTokenService jwt,
        string firstName,
        string lastName,
        string email,
        string password)
        {
            email = email.Trim().ToLowerInvariant();

            if (await db.Users.AnyAsync(u => u.Email == email))
                throw new GraphQLException("Email already exists.");

            var user = new User
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email,
                PasswordHash = hasher.Hash(password)
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return new AuthPayload(jwt.CreateToken(user), user);
        }

        public async Task<AuthPayload> Login(
            AppDbContext db,
            PasswordHasher hasher,
            JwtTokenService jwt,
            string email,
            string password)
        {
            email = email.Trim().ToLowerInvariant();

            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null || !hasher.Verify(password, user.PasswordHash))
                throw new GraphQLException("Invalid credentials.");

            return new AuthPayload(jwt.CreateToken(user), user);
        }
    }
}
