using Raylib_cs;

class Program
{
    static void Main()
    {

        Raylib.InitWindow(1200, 800, "Top down shooter");
        List<Enemy> enemies = new();
        Player p = new();
        for (int i = 0; i < 5; i++)
            enemies.Add(new Enemy());


        while (!Raylib.WindowShouldClose())
        {
            EnemyCheck(enemies, p);
            p.Update();
            if (p.IsDead()) p = new();

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


    static void EnemyCheck(List<Enemy> enemies, Player p)
    {

        foreach (var enemy in enemies)
        {
            enemy.Update(p);
            if (p.Collides(enemy))
            {
                p.Hp -= 1;
            }

            // Ta bort bullet och health från enemy
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

            // Check om enemy är död
            if (enemy.IsDead())
            {
                enemies.Remove(enemy);
                enemies.Add(new Enemy());
                break;
            }
        }
    }
}


