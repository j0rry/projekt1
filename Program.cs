using Raylib_cs;
using System.Collections.Generic;
using System.IO;

class Program
{
    static string playerName = "";
    static bool isEnteringName = false;

    static void Main()
    {
        Raylib.InitWindow(1200, 800, "Top down shooter");
        List<Enemy> enemies = new();
        bool IsGameOver = false;
        Player p = new();
        for (int i = 0; i < 5; i++)
            enemies.Add(new Enemy());

        while (!Raylib.WindowShouldClose())
        {
            if (isEnteringName)
            {
                AskUsername(ref playerName, () =>
                {
                    p = new();
                    isEnteringName = false;
                    IsGameOver = false;
                });
            }
            else
            {
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
        Raylib.ClearBackground(Color.Blue);
        Raylib.DrawText(input, 400, 350, 20, Color.Black);
        Raylib.EndDrawing();
    }


    static void EnemyCheck(List<Enemy> enemies, Player p)
    {
        foreach (var enemy in enemies)
        {
            enemy.Update(p);
            if (p.Collides(enemy))
            {
                p.Hp -= 1;
            }

            if (p.Bullets.Count > 0)
            {
                foreach (var bullet in p.Bullets)
                {
                    if (enemy.BulletCollision(bullet))
                    {
                        enemy.Hp -= p.Damage;
                        p.Bullets.Remove(bullet);
                        break;
                    }
                }
            }

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