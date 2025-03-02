using Raylib_cs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Program
{
    // Globala variabler för att hantera spelets tillstånd
    static string playerName = "";
    static bool isEnteringName = false;
    static bool isScoreboard = true;

    const string path = "scoreboard.json";

    // Huvudmetoden som kör spelet
    static void Main()
    {
        Raylib.InitWindow(1200, 800, "Top down shooter");
        List<Enemy> enemies = new(); // Används för att kunna lagra fiender dynamiskt
        bool IsGameOver = false;
        Player p = new();
        for (int i = 0; i < 5; i++)
            enemies.Add(new Enemy());

        // Huvudloopen som kör spelet tills fönstret stängs
        while (!Raylib.WindowShouldClose())
        {
            if (isScoreboard)
            {
                HandleScoreboard(ref isScoreboard);
            }
            else if (isEnteringName)
            {
                // Hanterar inmatnig av användarnamn
                AskUsername(ref playerName, () =>
                {
                    // Sparar spelarens score samt återställer spelets tillstånd.
                    SaveData(p);
                    p = new();
                    isEnteringName = false;
                    IsGameOver = false;
                    isScoreboard = true;
                });
            }
            else
            {
                // Uppdaterar fiender och spelare
                EnemyCheck(enemies, p);

                if (!IsGameOver)
                    p.Update();

                if (p.IsDead())
                {
                    IsGameOver = true;
                    isEnteringName = true;
                }

                Raylib.SetTargetFPS(60);
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Green);
                p.Draw();
                p.DrawHealthBar();
                enemies.ForEach(enemy => enemy.Draw());
                Raylib.EndDrawing();
            }
        }
        Raylib.UnloadTexture(p.texture);
        Raylib.CloseWindow();
    }

    // Metod för att hantera inmatning av användarnamn
    static void AskUsername(ref string input, Action onEnter)
    {
        int key = Raylib.GetKeyPressed();
        if (key > 32 && key <= 125 && input.Length < 12)
        {
            input += (char)key;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && input.Length > 0)
        {
            input = input.Substring(0, input.Length - 1);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Enter) && input.Length > 0)
        {
            onEnter();
        }

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawText("Username: ", 290, 350, 20, Color.White);
        Raylib.DrawText(input, 400, 350, 20, Color.White);
        Raylib.EndDrawing();
    }

    // Metod för att spara spelarens data
    static void SaveData(Player p)
    {
        // Skapar en lista för att lagra highscores / spelar data
        List<PlayerData> playerDataList = new List<PlayerData>();

        if (File.Exists(path))
        {
            try
            {
                // Läser befintlig JSON data från filen
                string existingJson = File.ReadAllText(path);
                // Deserialiserar JSON data till en lista av PlayerData objekt
                playerDataList = JsonSerializer.Deserialize<List<PlayerData>>(existingJson) ?? new List<PlayerData>();
            }
            catch (JsonException ex)
            {
                // Hanterar fel
                Console.WriteLine($"Error reading JSON: {ex.Message}");
            }
        }

        // Skapar ett nytt PlayerData objekt med spelarens namn och antal kills
        var playerData = new PlayerData
        {
            Name = playerName,
            Kills = p.KillCount
        };
        // Lägger till den nya spelarens data i listan
        playerDataList.Add(playerData);

        try
        {
            // Serialiserar listan av playerDatan till JSON
            string jsonString = JsonSerializer.Serialize(playerDataList);
            // Skriver JSON data till filen
            File.WriteAllText(path, jsonString);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error writing JSON: {ex.Message}");
        }
    }

    // Metod för att visa scoreboard
    static void ShowScoreboard()
    {
        if (File.Exists(path))
        {
            try
            {
                // läser json data från filen
                string jsonString = File.ReadAllText(path);
                // Deserialiserar JSON data till en lista av PlayerData objekt
                var playerDataList = JsonSerializer.Deserialize<List<PlayerData>>(jsonString);

                // Sorterar listan efter antal kills i fallande ordning och tar de 10 bästa resultaten
                var topScores = playerDataList.OrderByDescending(p => p.Kills).Take(10).ToList();


                int yPosition = 300;
                // Ritar varje spelares namn och antal kills
                foreach (var playerData in topScores)
                {
                    Raylib.DrawText($"{playerData.Name} - Kills: {playerData.Kills}", 400, yPosition, 20, Color.White);
                    yPosition += 30;
                }

            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error reading JSON: {ex.Message}");
            }
        }
    }

    static void HandleScoreboard(ref bool isScoreboard)
    {
        if (!File.Exists(path)) isScoreboard = false;

        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            isScoreboard = !isScoreboard;
        }

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        ShowScoreboard();
        Raylib.DrawText("Press Space to START", (Raylib.GetScreenWidth() - Raylib.MeasureText("Press Space to START", 20)) / 2, Raylib.GetScreenHeight() - 40, 20, Color.White);
        Raylib.EndDrawing();
    }

    static void EnemyCheck(List<Enemy> enemies, Player p)
    {
        // Loopar igenom alla fiender i listan
        foreach (var enemy in enemies)
        {
            // Uppdaterar fiendens position och tillstånd
            enemy.Update(p);

            // Kontrolerar om spelaren kolliderar med fienden
            if (p.Collides(enemy))
            {
                p.Hp -= 1;
            }

            // Kontrollerar om det finns några kulor i spelarens lista av Bullet
            if (p.Bullets.Count > 0)
            {
                foreach (var bullet in p.Bullets)
                {
                    // Kollar om kulan kolliderar med fienden
                    if (enemy.BulletCollision(bullet))
                    {
                        enemy.Hp -= p.Damage;
                        p.Bullets.Remove(bullet);
                        break;
                    }
                }
            }

            // Kontrollerar om fienden är död
            if (enemy.IsDead())
            {
                enemies.Remove(enemy);
                enemies.Add(new Enemy());
                p.KillCount++;
                break;
            }
        }
    }
}