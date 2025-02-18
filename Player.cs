using Raylib_cs;
using System.Numerics;

class Player
{
    public int X { get; set; } = Raylib.GetScreenWidth() / 2;
    public int Y { get; set; } = Raylib.GetScreenHeight() / 2;
    public int Radius { get; set; } = 30;
    public Raylib_cs.Color Color { get; set; } = Color.Blue;

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

    public Player()
    {
        texture = Raylib.LoadTexture("player.png");
    }

    private (float, float) CalculateGunPosition()
    {
        float gunX = X + GunOffsetX * MathF.Cos(RotationAngle * MathF.PI / 180) - GunOffsetY * MathF.Sin(RotationAngle * MathF.PI / 180);
        float gunY = Y + GunOffsetX * MathF.Sin(RotationAngle * MathF.PI / 180) + GunOffsetY * MathF.Cos(RotationAngle * MathF.PI / 180);
        return (gunX, gunY);
    }

    public void Update()
    {
        PlayerInput();
        Bullets.RemoveAll(bullet => bullet.X < 0 || bullet.X > Raylib.GetScreenWidth() || bullet.Y < 0 || bullet.Y > Raylib.GetScreenHeight());
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
        Vector2 origin = new Vector2((texture.Width * Scale / 2) - 27, texture.Height * Scale / 2);

        Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, RotationAngle, Color.White);

        (float gunX, float gunY) = CalculateGunPosition();

        foreach (var bullet in Bullets) // Rita alla bullets
        {
            bullet.Draw();
        }
    }

    void PlayerInput()
    {
        if (Raylib.IsKeyDown(KeyboardKey.W)) Y -= Speed;
        if (Raylib.IsKeyDown(KeyboardKey.S)) Y += Speed;
        if (Raylib.IsKeyDown(KeyboardKey.D)) X += Speed;
        if (Raylib.IsKeyDown(KeyboardKey.A)) X -= Speed;

        if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.J)) RotationAngle -= RotateSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.K)) RotationAngle += RotateSpeed;

        X = Math.Clamp(X, 0 + Radius, Raylib.GetScreenWidth() - Radius);
        Y = Math.Clamp(Y, 0 + Radius, Raylib.GetScreenHeight() - Radius);

        if (Raylib.IsKeyDown(KeyboardKey.LeftAlt))
        {
            Shoot();
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Space)) Hp--;
    }

    public void Shoot()
    {
        float currentTime = (float)Raylib.GetTime(); // Hämta tiden i sekunder
        if (currentTime - lastShotTime >= 1.0f / FireRate) // Om det gått 1/FireRate sekunder sedan senaste skottet
        {
            // Räkna ut positionen för skottet
            float gunX = X + GunOffsetX * MathF.Cos(RotationAngle * MathF.PI / 180) - GunOffsetY * MathF.Sin(RotationAngle * MathF.PI / 180);
            float gunY = Y + GunOffsetX * MathF.Sin(RotationAngle * MathF.PI / 180) + GunOffsetY * MathF.Cos(RotationAngle * MathF.PI / 180);

            Bullet bullet = new Bullet(gunX, gunY, RotationAngle);
            Bullets.Add(bullet);
            lastShotTime = currentTime;
        }
    }


    public void DrawHealthBar()
    {
        // Rita ut en healthbar
        int maxWidth = 300;
        float healthProcentage = (float)Hp / MaxHp;
        float healthBarWidth = maxWidth * healthProcentage;

        Raylib.DrawRectangle(10 - 5, Raylib.GetScreenHeight() - 65, maxWidth + 10, 60, Color.Black);
        Raylib.DrawRectangle(10, Raylib.GetScreenHeight() - 60, (int)healthBarWidth, 50, Color.Red);
    }

    public bool Collides(Enemy enemy)
    {
        // Kollar om fienden kolliderar med spelaren
        return MathF.Sqrt((X - enemy.X) * (X - enemy.X) + (Y - enemy.Y) * (Y - enemy.Y)) < Radius + enemy.Radius;
    }

}
