using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TeeMate_ServerSide;
using TeeMate_ServerSide.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TeeMateDbContext>(builder.Configuration["TeeMateDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// USER ENDPOINTS


// Create a User

app.MapPost("/api/user", (TeeMateDbContext db, User user) =>
{
    db.Users.Add(user);
    db.SaveChanges();
    return Results.Created($"/api/user/{user.Id}", user);
});


// Get All Users

app.MapGet("/api/users", (TeeMateDbContext db) =>
{
    List<User> users = db.Users.Include(t => t.TeeTimes).ToList();
    if (users.Count == 0)
    {
        return Results.NotFound("No users to be found.");
    }

    return Results.Ok(users);
});


// Get User by uid

app.MapGet("/api/user/{uid}", (TeeMateDbContext db, string uid) =>
{
    var user = db.Users.Single(u => u.Uid == uid);
    return user;
});


// Update a User

app.MapPut("/api/user/{id}", (TeeMateDbContext db, int id, User user) =>
{
    User UserToUpdate = db.Users.SingleOrDefault(user => user.Id == id);
    if (UserToUpdate == null)
    {
        return Results.NotFound("No user available.");
    }

    UserToUpdate.FirstName = user.FirstName;
    UserToUpdate.LastName = user.LastName;
    UserToUpdate.Age = user.Age;
    UserToUpdate.Handicap = user.Handicap;
    UserToUpdate.Availability = user.Availability;
    UserToUpdate.Transportation = user.Transportation;
    UserToUpdate.Clubs = user.Clubs;
    UserToUpdate.ProfilePic = user.ProfilePic;
    UserToUpdate.Uid = user.Uid;
    UserToUpdate.SkillLevelId = user.SkillLevelId;

    db.SaveChanges();
    return Results.NoContent();
});


// Delete A User



app.Run();
