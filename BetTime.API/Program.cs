using BetTime.Data;
using BetTime.Data;
using Microsoft.EntityFrameworkCore;

using BetTime.Business;
using System.Text.Json.Serialization;;

var builder = WebApplication.CreateBuilder(args);



// Servicios del negocio y repositorios
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISportService, SportService>();
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBetService, BetService>();
builder.Services.AddScoped<IUserRepository, UserEFRepository>();
builder.Services.AddScoped<ISportRepository, SportEFRepository>();
builder.Services.AddScoped<ILeagueRepository, LeagueEFRepository>();
builder.Services.AddScoped<ITeamRepository, TeamEFRepository>();
builder.Services.AddScoped<IMatchRepository, MatchEFRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionEFRepository>();
builder.Services.AddScoped<IBetRepository, BetEFRepository>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BetTimeContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowedOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddControllers();

builder.Services.AddHttpClient();


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<BetTimeContext>();
        context.Database.Migrate();
    }



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});

app.UseHttpsRedirection();
app.UseCors("MyAllowedOrigins");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();