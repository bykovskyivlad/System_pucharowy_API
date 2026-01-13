using System.Text;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System_pucharowy_API.Authozycja;
using System_pucharowy_API.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddHttpContextAccessor();

// JWT options + services
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<JwtTokenService>();

var jwtOpt = builder.Configuration.GetSection("Jwt").Get<JWTOptions>()!;


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                Console.WriteLine("AUTH HEADER: " + ctx.Request.Headers["Authorization"].ToString());
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("JWT FAIL: " + ctx.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("JWT OK (validated).");
                return Task.CompletedTask;
            }
        };

        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtOpt.Issuer,
            ValidAudience = jwtOpt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpt.Key)),

            
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();


builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<System_pucharowy_API.GraphQL.Query>()
    .AddMutationType<System_pucharowy_API.GraphQL.Mutation>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


app.UseAuthentication();
app.UseAuthorization();


app.MapGraphQL("/graphql");
app.MapGet("/", () => "Go to /graphql");

app.Run();
