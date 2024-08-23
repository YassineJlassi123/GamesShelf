using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

public class Game
{
    public int Id { get; set; }  // Primary Key
    public int userId { get; set; }
    public string game { get; set; }
    public int playTime { get; set; }
    public string genre { get; set; }
 
     public List<string> platforms { get; set; }
}