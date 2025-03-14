using Raylib_cs;
using System.Numerics;

class Player
{
    public int X { get; set; } = Raylib.GetScreenWidth() / 2;
    public int Y { get; set; } = Raylib.GetScreenHeight() / 2;
    public int Radius { get; set; } = 30;
    public Raylib_cs.Color Color { get; set; } = Color.Blue;

    public int Ammo = 9999;
    public int MaxHp { get; set; } = 100;
    public int Hp { get; set; } = 100;
    public int Speed { get; set; } = 5;
    public int FireRate { get; set; } = 10;
    public int Damage { get; set; } = 20;
    public int RotateSpeed { get; set; } = 5;

    public Texture2D texture;
    public float Scale { get; set; } = 0.5f;

    public float GunOffsetX { get; set; } = 90;
    public float GunOffsetY { get; set; } = 20;

    public List<Bullet> Bullets { get; set; } = new List<Bullet>();
    public float RotationAngle { get; set; }
    private float lastShotTime = 0;

    public int KillCount { get; set; } = 0;

    public Player()
    {
        texture = Raylib.LoadTexture("player.png");
    }


    public void Update()
    {
        PlayerInput();

        // Tar bort alla Bullet som är utanför skärmen
        Bullets.RemoveAll(bullet => bullet.X < 0 || bullet.X > Raylib.GetScreenWidth() || bullet.Y < 0 || bullet.Y > Raylib.GetScreenHeight());

        // Updatera Bullet
        foreach (var bullet in Bullets)
        {
            bullet.Update();
        }
    }

    public void Draw()
    {
        Vector2 playerPosition = new Vector2(X, Y);

        Rectangle sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);
        Rectangle destRec = new Rectangle(X, Y, texture.Width * Scale, texture.Height * Scale);
        
        // Ursprungspunkten för rotation, Som är centrerad på spelaren i texturen.
        Vector2 origin = new Vector2((texture.Width * Scale / 2) - 27, texture.Height * Scale / 2);

        Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, RotationAngle, Color.White);


        foreach (var bullet in Bullets) // Rita alla bullets
        {
            bullet.Draw();
        }

        Raylib.DrawText($"Ammo: {Ammo}", 0, 0, 40, Color.White);
        Raylib.DrawText($"Kill Count: {KillCount}", 0, 50, 40, Color.White);
    }

    void PlayerInput()
    {
        // Kontroller
        if (Raylib.IsKeyDown(KeyboardKey.W)) Y -= Speed;
        if (Raylib.IsKeyDown(KeyboardKey.S)) Y += Speed;
        if (Raylib.IsKeyDown(KeyboardKey.D)) X += Speed;
        if (Raylib.IsKeyDown(KeyboardKey.A)) X -= Speed;

        if (Raylib.IsKeyDown(KeyboardKey.R)) Ammo = 100;

        if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.J)) RotationAngle -= RotateSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.K)) RotationAngle += RotateSpeed;

        // Låser spelaren så den inte kan gå utanför spelplannen
        X = Math.Clamp(X, 0 + Radius, Raylib.GetScreenWidth() - Radius);
        Y = Math.Clamp(Y, 0 + Radius, Raylib.GetScreenHeight() - Radius);

        if (Raylib.IsKeyDown(KeyboardKey.LeftAlt))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        float currentTime = (float)Raylib.GetTime(); // Hämta tiden i sekunder
        if (currentTime - lastShotTime >= 1.0f / FireRate && Ammo > 0) // Om det gått 1/FireRate sekunder sedan senaste skottet
        {
            // Räkna ut positionen för skottet
            float gunX = X + GunOffsetX * MathF.Cos(RotationAngle * MathF.PI / 180) - GunOffsetY * MathF.Sin(RotationAngle * MathF.PI / 180);
            float gunY = Y + GunOffsetX * MathF.Sin(RotationAngle * MathF.PI / 180) + GunOffsetY * MathF.Cos(RotationAngle * MathF.PI / 180);

            Bullet bullet = new Bullet(gunX, gunY, RotationAngle);
            Bullets.Add(bullet);
            lastShotTime = currentTime;
            Ammo--;
        }
    }


    public void DrawHealthBar()
    {
        // Räkna ut healthbar
        int maxWidth = 300;
        float healthProcentage = (float)Hp / MaxHp;
        float healthBarWidth = maxWidth * healthProcentage;

        // Rita ut healthbar
        Raylib.DrawRectangle(10 - 5, Raylib.GetScreenHeight() - 65, maxWidth + 10, 60, Color.Black);
        Raylib.DrawRectangle(10, Raylib.GetScreenHeight() - 60, (int)healthBarWidth, 50, Color.Red);
    }

    public bool IsDead()
    {
        return Hp <= 0; // Kollar om spelaren är död
    }

    public bool Collides(Enemy enemy)
    {
        // Kollar om fienden kolliderar med spelaren
        return MathF.Sqrt((X - enemy.X) * (X - enemy.X) + (Y - enemy.Y) * (Y - enemy.Y)) < Radius + enemy.Radius;
    }

}
