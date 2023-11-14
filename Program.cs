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



// TEE TIME ENDPOINTS


// Get All Tee Times

app.MapGet("/api/teeTimes", (TeeMateDbContext db) =>
{
    List<TeeTime> teeTimes = db.TeeTimes.Include(t => t.Users).ToList();
    if (teeTimes.Count == 0)
    {
        return Results.NotFound("No Tee Times found.");
    }

    return Results.Ok(teeTimes);
});


// Get Single Tee Time

app.MapGet("/api/teeTimes/{id}", (TeeMateDbContext db, int id) =>
{
    TeeTime teeTime = db.TeeTimes
    .Include(t => t.Users)
    .FirstOrDefault(t => t.Id == id);
    if (teeTime == null)
    {
        return Results.NotFound("No User found.");
    }

    return Results.Ok(teeTime);
});


// Get User's Tee Time

app.MapGet("/api/teeTimeUser/{uid}", (TeeMateDbContext db, string uid) =>
{
    var user = db.Users
    .Include(u => u.TeeTimes)
    .ThenInclude(t => t.SkillLevel)
    .FirstOrDefault(u => u.Uid == uid);

    if (user == null)
    {
        return Results.NotFound("User not found");
    }

    var teeTimes = user.TeeTimes.ToList();
    return Results.Ok(teeTimes);
});


// Create Tee Time

app.MapPost("/api/teeTimes", (TeeMateDbContext db, TeeTime teeTime) =>
{
    try
    {
        db.Add(teeTime);
        db.SaveChanges();
        return Results.Created($"/api/teeTimes/{teeTime.Id}", teeTime);
    }
    catch (DbUpdateException)
    {
        return Results.NotFound("Issue creating Tee Time.");
    }
});


// Update Tee Time

app.MapPut("/api/teeTimes/{teeTimeId}", (TeeMateDbContext db, int teeTimeId, TeeTime teeTime) =>
{
    TeeTime updateTeeTime = db.TeeTimes.SingleOrDefault(t => t.Id == teeTimeId);
    if (updateTeeTime == null)
    {
        return Results.NotFound("No Tee Time to update.");
    }
    updateTeeTime.Date = teeTime.Date;
    updateTeeTime.Time = teeTime.Time;
    updateTeeTime.Location = teeTime.Location;
    updateTeeTime.NumOfPlayers = teeTime.NumOfPlayers;
    updateTeeTime.CourseId = teeTime.CourseId;

    db.SaveChanges();
    return Results.NoContent();
});


// Delete Tee Time

app.MapDelete("/api/teeTimes/{teeTimeId}", (TeeMateDbContext db, int teeTimeId) =>
{
    TeeTime deleteTeeTime = db.TeeTimes.FirstOrDefault(t => t.Id == teeTimeId);
    if (deleteTeeTime == null)
    {
        return Results.NotFound("No Tee Time found to delete.");
    }

    db.Remove(deleteTeeTime);
    db.SaveChanges();
    return Results.Ok(deleteTeeTime);
});


// Add User to Tee Time

app.MapPost("/api/teeTimeUser/{teeTimeId}/{userId}", (TeeMateDbContext db, int teeTimeId, int userId) =>
{
    var teeTime = db.TeeTimes
    .Include(t => t.Users)
    .FirstOrDefault(t => t.Id == teeTimeId);

    var user = db.Users.FirstOrDefault(u => u.Id == userId);

    if (teeTime == null || user == null)
    {
        return Results.NotFound("Tee Time or User not found.");
    }

    if (teeTime.Users.Any(u => u.Id == userId))
    {
        return Results.BadRequest("User has already joined this Tee Time.");
    }

    teeTime.Users.Add(user);
    db.SaveChanges();

    return Results.Ok(user);
});

app.Run();
