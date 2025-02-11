using Raylib_cs;
using System.Numerics;

class Player
{
    public int X { get; set; } = Raylib.GetScreenWidth() / 2;
    public int Y { get; set; } = Raylib.GetScreenHeight() / 2;
    public int Radius { get; set; } = 40;
    public Raylib_cs.Color Color { get; set; } = Color.Blue;

    public int Hp { get; set; } = 100;
    public int Speed { get; set; } = 5;
    public int FireRate { get; set; } = 5;
    public int Damage { get; set; } = 20;

    public Texture2D texture;
    public float Scale { get; set; } = 0.5f;

    public float GunOffsetX { get; set; } = 90;
    public float GunOffsetY { get; set; } = 20;

    public Player()
    {
        texture = Raylib.LoadTexture("player.png");
    }

    public void Update()
    {
        Movement();
    }

    public void Draw()
    {
        Raylib.DrawCircle(X, Y, Radius, Color); // Debug Hitbox

    
        Vector2 playerPosition = new Vector2(X, Y);
        Vector2 mousePosition = Raylib.GetMousePosition();
        float angle = MathF.Atan2(mousePosition.Y - playerPosition.Y, mousePosition.X - playerPosition.X) * (180.0f / MathF.PI);

        Rectangle sourceRec = new Rectangle(0, 0, texture.Width, texture.Height);

        Rectangle destRec = new Rectangle(X, Y, texture.Width * Scale, texture.Height * Scale);

        Vector2 origin = new Vector2((texture.Width * Scale / 2) - 27, texture.Height * Scale / 2);
        Raylib.DrawTexturePro(texture, sourceRec, destRec, origin, angle, Color.White);

        
        float gunX = X + GunOffsetX * MathF.Cos(angle * MathF.PI / 180) - GunOffsetY * MathF.Sin(angle * MathF.PI / 180);
        float gunY = Y + GunOffsetX * MathF.Sin(angle * MathF.PI / 180) + GunOffsetY * MathF.Cos(angle * MathF.PI / 180);
        Raylib.DrawCircle((int)gunX, (int)gunY, 5, Color.Red); // Debug bullet spawn


    }

    void Movement()
    {
        if (Raylib.IsKeyDown(KeyboardKey.W)) Y -= Speed;
        if (Raylib.IsKeyDown(KeyboardKey.S)) Y += Speed;
        if (Raylib.IsKeyDown(KeyboardKey.D)) X += Speed;
        if (Raylib.IsKeyDown(KeyboardKey.A)) X -= Speed;


        X = Math.Clamp(X, 0 + Radius, Raylib.GetScreenWidth() - Radius);
        Y = Math.Clamp(Y, 0 + Radius, Raylib.GetScreenHeight() - Radius);
    }
}