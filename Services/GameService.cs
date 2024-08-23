using Microsoft.EntityFrameworkCore;

public class GameService : IGameService
{
    private readonly GameDbContext _context;

    public GameService(GameDbContext context)
    {
        _context = context;
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        return await _context.Games.ToListAsync();
    }

    public async Task<Game> GetGameByIdAsync(int id)
    {
        return await _context.Games.FindAsync(id);
    }

    public async Task<Game> UpdateGameAsync(int id, Game game)
    {
        var existingGame = await _context.Games.FindAsync(id);
        if (existingGame == null) return null;

        existingGame.userId = game.userId;
        existingGame.game = game.game;
        existingGame.playTime = game.playTime;
        existingGame.genre = game.genre;
        existingGame.platforms = game.platforms;

        await _context.SaveChangesAsync();
        return existingGame;
    }

    public async Task DeleteGameAsync(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<List<GamePlaytimeSummary>> SelectTopByPlaytimeAsync(string genre, string platform)
    {
        // Build the query with optional filters
        var query = _context.Games.AsQueryable();

        if (!string.IsNullOrEmpty(genre))
        {
            // Use ToLower to enable case-insensitive comparison
            query = query.Where(g => g.genre.ToLower() == genre.ToLower());
        }

        // Retrieve the data and perform the platform filter in memory
        var games = await query.ToListAsync();

        if (!string.IsNullOrEmpty(platform))
        {
            games = games.Where(g => g.platforms != null && g.platforms.Any(p => p.ToLower() == platform.ToLower())).ToList();
        }

        // Group by game name and calculate total playtime
        var topGames = games
            .GroupBy(g => g.game)
            .Select(g => new GamePlaytimeSummary
            {
                Game = g.Key,
                TotalPlayTime = g.Sum(game => game.playTime)
            })
            .OrderByDescending(g => g.TotalPlayTime)
            .ToList();

        return topGames;
    }
    public async Task<List<TotalplayerSummary>> SelectTopByPlayersAsync(string genre = null, string platform = null)
    {
        // Build the query with optional filters
        var query = _context.Games.AsQueryable();

        if (!string.IsNullOrEmpty(genre))
        {
            // Use ToLower to enable case-insensitive comparison
            query = query.Where(g => g.genre.ToLower() == genre.ToLower());
        }

        // Retrieve the data and perform the platform filter in memory
        var games = await query.ToListAsync();

        if (!string.IsNullOrEmpty(platform))
        {
            games = games.Where(g => g.platforms != null && g.platforms.Any(p => p.ToLower() == platform.ToLower())).ToList();
        }

        // Group by game name and calculate unique player count
        var topGames = games
            .GroupBy(g => g.game)
            .Select(g => new TotalplayerSummary
            {
                Game = g.Key,
                TotalPlayTime = g.Sum(game => game.playTime),
                TotalPlayers = g.Select(game => game.userId).Distinct().Count()  // Count unique players
            })
            .OrderByDescending(g => g.TotalPlayers)
            .ToList();

        return topGames;
    }
}
public class GamePlaytimeSummary
{
    public string Game { get; set; }
    public int TotalPlayTime { get; set; }
}
    public class TotalplayerSummary
    {
        public string Game { get; set; }
        public int TotalPlayTime { get; set; }
        public int TotalPlayers { get; set; }
    }
