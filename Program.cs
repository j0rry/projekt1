using Raylib_cs;

class Program
{
    static void Main()
    {

        Raylib.InitWindow(1200, 800, "Top down shooter");
        Player p = new();
        List<Enemy> enemies = new();
        for (int i = 0; i < 5; i++)
        {
            enemies.Add(new Enemy());
        }

        while (!Raylib.WindowShouldClose())
        {
            p.Update();

            if (p.Hp <= 0)
            {
                p = new();
            }

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

                if (enemy.Hp <= 0)
                {
                    enemies.Remove(enemy);
                    enemies.Add(new Enemy());
                    break;
                }
            }

            Raylib.SetTargetFPS(60);
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Green);
            p.Draw();
            p.DrawHealthBar();
            enemies.ForEach(enemy => enemy.Draw());
            Raylib.EndDrawing();

        }
        Raylib.UnloadTexture(p.texture);
        Raylib.CloseWindow();


    }
}


