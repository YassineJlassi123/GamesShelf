# 🎮 GamesShelf

**GamesShelf** is a robust backend application for managing and analyzing your video game collection. Built with ASP.NET Core and Entity Framework Core, it provides features to track and gain insights from your games.

## 🚀 Features

- **Manage Your Games**: Add, update, view, and delete games.
- **Playtime Analysis**: Analyze total playtime by game, genre, or platform.
- **Player Statistics**: Track unique player counts for each game.
- **Custom Queries**: Filter games by genre and platform for detailed insights.
  
## Tech Stack

- **Backend:** ASP.NET Core 8
- **Database:** PostgreSQL (or your preferred DB)
- **Hosting:** Render
- **Version Control:** Git

## 🛠️ Getting Started

### Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- PostgresQl

### Setup

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/yourusername/GamesShelf.git
    cd GamesShelf
    ```

2. **Install Dependencies**:
    ```bash
    dotnet restore
    ```

3. **Configure Your Database**:
   - Modify `appsettings.json` with your Postgres connection string.

4. **Apply Database Migrations**:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

5. **Run the Application**:
    ```bash
    dotnet run
    ```

## 📚 API Endpoints

- **Create a Game**: `POST /games`
- **Get All Games**: `GET /games`
- **Get Game by ID**: `GET /games/{id}`
- **Update a Game**: `PUT /games/{id}`
- **Delete a Game**: `DELETE /games/{id}`
- **Top Games by Playtime**: `GET /games/top-playtime`
- **Top Games by Players**: `GET /games/top-players`

## 🧩 Handling Challenges

### 🛠️ Required Collections

To address issues with required collections (e.g., `Platforms` in the `Game` class), always initialize collections to avoid null reference problems:
```csharp
public class Game
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public int PlayTime { get; set; }
    public string Genre { get; set; }
    public List<string> Platforms { get; set; } = new List<string>(); // Always initialized
}
```
## 🧪 Mocking Database Operations

For unit testing, use mocks to simulate database operations. For instance, mock `FindAsync`:

```csharp
var mockSet = new Mock<DbSet<Game>>();
mockSet.Setup(m => m.FindAsync(It.IsAny<int>())).Returns<int>(id => Task.FromResult(games.FirstOrDefault(g => g.Id == id)));
var query = _context.Games.AsQueryable();

if (!string.IsNullOrEmpty(genre))
{
    query = query.Where(g => g.Genre.ToLower() == genre.ToLower());
}

var games = await query.ToListAsync();

if (!string.IsNullOrEmpty(platform))
{
    games = games.Where(g => g.Platforms != null && g.Platforms.Any(p => p.ToLower() == platform.ToLower())).ToList();
}

var topGames = games
    .GroupBy(g => g.GameName)
    .Select(g => new GamePlaytimeSummary
    {
        Game = g.Key,
        TotalPlayTime = g.Sum(game => game.PlayTime)
    })
    .OrderByDescending(g => g.TotalPlayTime)
    .ToList();
```
##Production URL: You can test the live application at:https://gamesshelf.onrender.com
## 📫 Contact

For inquiries, please reach out to [yassinej696@gmail.com](mailto:yassinej696@gmail.com).
