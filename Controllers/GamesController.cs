using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="game">The game to create.</param>
    /// <returns>The created game.</returns>
    [HttpPost]
    public async Task<ActionResult<Game>> CreateGame([FromBody] Game game)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdGame = await _gameService.CreateGameAsync(game);
        return CreatedAtAction(nameof(GetGameById), new { id = createdGame.Id }, createdGame);
    }

    /// <summary>
    /// Retrieves all games.
    /// </summary>
    /// <returns>A list of games.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetAllGames()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }

    /// <summary>
    /// Retrieves a game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game.</param>
    /// <returns>The game with the specified identifier.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGameById([FromRoute] int id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }

    /// <summary>
    /// Updates an existing game.
    /// </summary>
    /// <param name="id">The unique identifier of the game to update.</param>
    /// <param name="game">The updated game data.</param>
    /// <returns>The updated game.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Game>> UpdateGame([FromRoute] int id, [FromBody] Game game)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedGame = await _gameService.UpdateGameAsync(id, game);
        if (updatedGame == null)
        {
            return NotFound();
        }
        return Ok(updatedGame);
    }

    /// <summary>
    /// Deletes a game by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the game to delete.</param>
    /// <returns>Result of the delete operation.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame([FromRoute] int id)
    {
        await _gameService.DeleteGameAsync(id);
        return NoContent();
    }
    /// <summary>
    /// Retrieves the most played games with optional genre and platform filters.
    /// </summary>
    /// <param name="genre">Optional genre to filter by.</param>
    /// <param name="platform">Optional platform to filter by.</param>
    /// <returns>A list of games with the highest total playtime.</returns>
    [HttpGet("top-by-playtime")]
    public async Task<ActionResult<IEnumerable<GamePlaytimeSummary>>> GetTopByPlaytime([Required, FromQuery] string genre,
        [Required, FromQuery] string platform)
    {
        var games = await _gameService.SelectTopByPlaytimeAsync(genre, platform);
        return Ok(games);
    }
    /// <summary>
    /// Retrieves the games with the highest number of unique players, with optional genre and platform filters.
    /// </summary>
    /// <param name="genre">Optional genre to filter by.</param>
    /// <param name="platform">Optional platform to filter by.</param>
    /// <returns>A list of games with the highest number of unique players.</returns>
    [HttpGet("top-by-players")]
    public async Task<ActionResult<IEnumerable<TotalplayerSummary>>> GetTopByPlayers([Required, FromQuery] string genre,
        [Required, FromQuery] string platform)
    {
        var games = await _gameService.SelectTopByPlayersAsync(genre, platform);
        return Ok(games);
    }
}
