using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using TeeMate_ServerSide;
using TeeMate_ServerSide.Models;
using System.Text.Json.Serialization;

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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:7183")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

var app = builder.Build();

//Add for Cors 
app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();


// USER ENDPOINTS


//Check if a user exists
app.MapGet("/checkuser/{uid}", (TeeMateDbContext db, string uid) =>
{
    var user = db.Users.Where(x => x.Uid == uid).ToList();
    if (uid == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(user);
    }
});

// Create a User

app.MapPost("/api/users", (TeeMateDbContext db, User user) =>
{
    db.Users.Add(user);
    db.SaveChanges();
    return Results.Created($"/api/user/{user.Id}", user);
});


// Get All Users

app.MapGet("/api/users", (TeeMateDbContext db) =>
{
    List<User> usersWithTeeTimes = db.Users
    .Include(ttu => ttu.TeeTimeUsers)
    .ThenInclude(u => u.TeeTime)
    .ToList();

    if (usersWithTeeTimes.Count == 0)
    {
        return Results.NotFound("No users to be found.");
    }

    return Results.Ok(usersWithTeeTimes);
});


// Get User by Uid

//app.MapGet("/api/user/{uid}", (TeeMateDbContext db, string uid) =>
//{
//    var user = db.Users.SingleOrDefault(u => u.Uid == uid);
//    return user;
//});


// Get Single User by Id

app.MapGet("/api/users/{id}", (TeeMateDbContext db, int id) =>
{
    var user = db.Users
    .Include(ttu => ttu.TeeTimeUsers)
    .ThenInclude(u => u.TeeTime)
    .SingleOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Results.NotFound("User not found.");
    }

    return Results.Ok(user);
});

// Update a User

app.MapPut("/api/users/{id}", (TeeMateDbContext db, int id, User user) =>
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
    List<TeeTime> teeTimes = db.TeeTimes
    .Include(ttu => ttu.TeeTimeUsers)
    .ThenInclude(u => u.User)
    .ToList();
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
    .Include(ttu => ttu.TeeTimeUsers)
    .ThenInclude(t => t.User)
    .FirstOrDefault(t => t.Id == id);
    if (teeTime == null)
    {
        return Results.NotFound("No User found.");
    }

    return Results.Ok(teeTime);
});


// Get User's Tee Time

app.MapGet("/api/teeTimeUsers/{userId}", (TeeMateDbContext db, int userId) =>
{
    var user = db.TeeTimes.Where(u => u.UserId == userId);

    if (user == null)
    {
        return Results.NotFound("User not found");
    }

    return Results.Ok(user);
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
        return Results.NotFound();
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

app.MapPost("/api/teeTimeUsers/{teeTimeId}/{userId}", (TeeMateDbContext db, int teeTimeId, int userId, TeeTimeUser teeTimeUser) =>
{
    teeTimeUser.UserId = userId;
    teeTimeUser.TeeTimeId = teeTimeId;

    if (teeTimeUser == null)
    {
        return Results.NotFound("Tee Time or User not found.");
    }

    db.TeeTimeUsers.Add(teeTimeUser);
    db.SaveChanges();

    return Results.Created($"/api/teeTimeUsers/{teeTimeUser.Id}", teeTimeUser);
});


// Delete User from Tee Time

app.MapDelete("/api/teeTimeUser/{id}/{uid}", (TeeMateDbContext db, int id, string uid) =>
{
    var teeTime = db.TeeTimes.Where(tt => tt.Id == id).Include(I => I.Users).FirstOrDefault();
    var user = db.Users.Where(u => u.Uid == uid).FirstOrDefault();
    if (teeTime == null)
    {
        return Results.NotFound("Not Found");
    }

    teeTime.Users.Remove(user);
    db.SaveChanges();
    return Results.NoContent();
});



// SKILL LEVEL ENDPOINTS


// Get Skill Levels

app.MapGet("/api/skillLevels", (TeeMateDbContext db) =>
{
    List<SkillLevel> skillLevels = db.SkillLevels.ToList();
    if (skillLevels.Count == 0)
    {
        return Results.NotFound();
    }

    return Results.Ok(skillLevels);
});

app.Run();
