using Microsoft.EntityFrameworkCore;
using LicentaEnityFrameworkAPI.Datas;
using LicentaEntityFrameworkConsole.Models;
using Npgsql;
using StackExchange.Profiling;
using System.Data;

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
            var mp = MiniProfiler.StartNew("InsertTeam");

            Team team = new Team()
            {
                City = teamData.City,
                Name = teamData.Name,
                Country = teamData.Country,
                Points = teamData.Points
            };

            using (mp.Step("Execute query"))
            {
                dbContext.Teams.Add(team);
                dbContext.SaveChanges();
            }
            mp.Stop();
            Console.WriteLine(mp.RenderPlainText());

        }

        public void UpdateTeam(TeamData teamData)
        {
            var mp = MiniProfiler.StartNew("UpdateTeam");

            var sqlUpdate = "UPDATE licenta.teams SET points = @points WHERE name = @name";

            using (mp.Step("Execute query"))
            {
                dbContext.Database.ExecuteSqlRaw(sqlUpdate, new NpgsqlParameter("@points", teamData.Points), new NpgsqlParameter("@name", teamData.Name));
            }
            mp.Stop();
            Console.WriteLine(mp.RenderPlainText());
        }

        public void DeletePlayer(string position, long age)
        {
            var mp = MiniProfiler.StartNew("DeletePlayer");

            var sqlDelete = "DELETE FROM licenta.players WHERE position = @position AND age > @age";
            using (mp.Step("Execute query"))
            {
                dbContext.Database.ExecuteSqlRaw(sqlDelete, new NpgsqlParameter("@position", position), new NpgsqlParameter("@age", age));
            }
            mp.Stop();
            Console.WriteLine(mp.RenderPlainText());
        }

        public int GetPlayerAndTeamByPosition(string position)
        {
            int players;

            var mp = MiniProfiler.StartNew("GetPlayerAndTeamByPosition");

            var sqlDelete = @"SELECT players.*, teams.country, teams.city, teams.points FROM licenta.players INNER JOIN licenta.teams ON players.team = teams.name
                       WHERE players.position = @position AND players.team LIKE '%United'";

            using (mp.Step("Execute query"))
            {
                players = dbContext.Players.FromSqlRaw(sqlDelete, new NpgsqlParameter("@position", position)).ToList().Count;
            }
            mp.Stop();
            Console.WriteLine(mp.RenderPlainText());

            return players;
        }

        public int GetAllPlayers()
        {
            int players;

            var mp = MiniProfiler.StartNew("GetAllPlayers");

            using (mp.Step("Execute query"))
            {
                players = dbContext.Players.OrderBy(p => p.Id).ToList().Count;
            }
            mp.Stop();
            Console.WriteLine(mp.RenderPlainText());
            return players;
        }

        public int GetTeamsByPoints(long points)
        {
            int teams;

            var mp = MiniProfiler.StartNew("GetTeamsByPoints");

            using (mp.Step("Execute query"))
            {
                teams = dbContext.Teams.Where(t => t.Points > points).ToList().Count;
            }

            mp.Stop();
            Console.WriteLine(mp.RenderPlainText());
            return teams;
        }

    }
}
