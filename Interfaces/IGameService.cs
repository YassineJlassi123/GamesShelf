public interface IGameService
{
    Task<Game> CreateGameAsync(Game game);
    Task<List<Game>> GetAllGamesAsync();
    Task<Game> GetGameByIdAsync(int id);
    Task<Game> UpdateGameAsync(int id, Game game);
    Task DeleteGameAsync(int id);
    Task<List<GamePlaytimeSummary>> SelectTopByPlaytimeAsync(string genre = null, string platform = null);
    Task<List<TotalplayerSummary>> SelectTopByPlayersAsync(string genre = null, string platform = null);  // New method

}
