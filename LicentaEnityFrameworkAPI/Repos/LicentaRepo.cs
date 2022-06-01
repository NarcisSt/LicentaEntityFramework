using Microsoft.EntityFrameworkCore;
using LicentaEnityFrameworkAPI.Datas;
using LicentaEntityFrameworkConsole.Models;
using System.Data.SqlClient;
using Npgsql;

namespace LicentaEnityFrameworkAPI.Repos
{

    public class LicentaRepo : ILicentaRepo
    {
        private readonly PostgresContext dbContext;

        public LicentaRepo(PostgresContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InsertTeam(TeamData teamData)
        {
            Team team = new Team()
            {
                City = teamData.City,
                Name = teamData.Name,
                Country = teamData.Country,
                Points = teamData.Points
            };

            dbContext.Teams.Add(team);
            dbContext.SaveChanges();
        }

        public void UpdateTeam(TeamData teamData)
        {
            var sqlUpdate = "UPDATE licenta.teams SET points = @points WHERE name = @name";

            dbContext.Database.ExecuteSqlRaw(sqlUpdate,  new NpgsqlParameter("@points", teamData.Points), new NpgsqlParameter("@name", teamData.Name) );
        }

        public void DeletePlayer(string position, long age)
        {
            var sqlDelete = "DELETE FROM licenta.players WHERE position = @position AND age > @age";

            dbContext.Database.ExecuteSqlRaw(sqlDelete, new NpgsqlParameter("@position", position), new NpgsqlParameter("@age", age));
        }

        public int GetPlayerAndTeamByPosition(string position)
        {
            var sqlDelete = @"SELECT players.*, teams.country, teams.city, teams.points FROM licenta.players INNER JOIN licenta.teams ON players.team = teams.name
                       WHERE players.position = @position AND players.team LIKE '%United'";

            var playerList=  dbContext.Players.FromSqlRaw(sqlDelete, new NpgsqlParameter("@position", position)).ToList();

            return playerList.Count;
        }

        public int GetAllPlayers()
        {
            var players = dbContext.Players.OrderBy(p => p.Id).ToList();

            return players.Count;
        }

        public int GetTeamsByPoints(long points)
        {
            var teams = dbContext.Teams.Where(t => t.Points > points).ToList();

            return teams.Count;
        }

    }
}
