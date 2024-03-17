using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RockPaperScissorsApi;
using RockPaperScissorsApi.DAL;
using RockPaperScissorsApi.Entities;
using RockPaperScissorsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.Bind("Project", new Config());

builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseNpgsql(Config.ConnectionString));

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

builder.Services.InitializeRepositories();

builder.Services.InitializeServices();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
