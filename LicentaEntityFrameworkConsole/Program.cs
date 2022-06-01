// See https://aka.ms/new-console-template for more information
using LicentaEntityFrameworkConsole.Models;

Console.WriteLine("Hello, World!");

ReadTeams();
InsertTeam();

Console.WriteLine("");

ReadTeams();

static void ReadTeams() {

    var dbContext = new PostgresContext();
    var teams = dbContext.Teams.ToList();

    foreach (var team in teams) { 
        
        Console.WriteLine(team.Name);
    }
}

static void InsertTeam()
{

    var dbContext = new PostgresContext();
    Team team = new Team();
    team.Name = "Liverpool";
    team.City = "Liverpool";
    team.Country = "England";
    team.Points = 91;

    dbContext.Teams.Add(team);
    dbContext.SaveChanges();

    
}