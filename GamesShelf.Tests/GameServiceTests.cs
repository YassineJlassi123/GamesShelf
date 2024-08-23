using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GamesShelf.Tests
{
    public class GameServiceTests
    {
        private readonly GameService _service;
        private readonly GameDbContext _context;
        
        public GameServiceTests()
        {
            // Use a unique database name for each test run to avoid conflicts
            var options = new DbContextOptionsBuilder<GameDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new GameDbContext(options);
            _service = new GameService(_context);

            // Seed data
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var games = new List<Game>
            {
                new Game { Id = 1, userId = 1, game = "Game1", playTime = 100, genre = "Action", platforms = new List<string> { "PC" } },
                new Game { Id = 2, userId = 2, game = "Game2", playTime = 200, genre = "Adventure", platforms = new List<string> { "Console" } }
            };

            _context.Games.AddRange(games);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateGameAsync_ShouldAddGameAndSaveChanges()
        {
            // Arrange
            var newGame = new Game
            {
                Id = 3,
                userId = 3,
                game = "Test Game",
                playTime = 150,
                genre = "Strategy",
                platforms = new List<string> { "PC", "Console" }
            };

            // Act
            var result = await _service.CreateGameAsync(newGame);

            // Assert
            result.Should().BeEquivalentTo(newGame);
            _context.Games.Should().Contain(g => g.Id == 3);
        }

        [Fact]
        public async Task GetAllGamesAsync_ShouldReturnAllGames()
        {
            // Act
            var result = await _service.GetAllGamesAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(g => g.game == "Game1");
            result.Should().Contain(g => g.game == "Game2");
        }

        [Fact]
        public async Task GetGameByIdAsync_ShouldReturnGame()
        {
            // Act
            var result = await _service.GetGameByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new Game { Id = 1, userId = 1, game = "Game1", playTime = 100, genre = "Action", platforms = new List<string> { "PC" } });
        }

        [Fact]
        public async Task UpdateGameAsync_ShouldUpdateGame()
        {
            // Arrange
            var updatedGame = new Game
            {
                Id = 1,
                userId = 1,
                game = "Updated Game",
                playTime = 120,
                genre = "Action",
                platforms = new List<string> { "PC", "Console" }
            };

            // Act
            var result = await _service.UpdateGameAsync(1, updatedGame);

            // Assert
            result.Should().NotBeNull();
            result.game.Should().Be("Updated Game");
        }

        [Fact]
        public async Task DeleteGameAsync_ShouldRemoveGame()
        {
            // Act
            await _service.DeleteGameAsync(2);

            // Assert
            var game = await _context.Games.FindAsync(2);
            game.Should().BeNull();
        }

        [Fact]
        public async Task SelectTopByPlaytimeAsync_ShouldReturnFilteredGames()
        {
            // Act
            var result = await _service.SelectTopByPlaytimeAsync("Action", "PC");

            // Assert
            result.Should().ContainSingle(g => g.Game == "Game1");
        }

        [Fact]
        public async Task SelectTopByPlayersAsync_ShouldReturnFilteredGames()
        {
            // Act
            var result = await _service.SelectTopByPlayersAsync("Action", "PC");

            // Assert
            result.Should().ContainSingle(g => g.Game == "Game1");
        }
    }
}
