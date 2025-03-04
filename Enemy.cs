using Raylib_cs;
using System.Numerics;

class Enemy
{
    public int Hp { get; set; } = 100;
    public int MaxHp { get; set; } = 100;

    public int X { get; set; } = Random.Shared.Next(Raylib.GetScreenWidth());
    public int Y { get; set; } = Random.Shared.Next(Raylib.GetScreenHeight());
    public int Radius { get; set; } = 20;
    public int Speed { get; set; } = 2;

    public Texture2D texture;

    public Enemy()
    {
        //texture = Raylib.LoadTexture("enemy.png");
    }

    public void Draw()
    {
        // Rita enemy
        Raylib.DrawCircle(X, Y, Radius, Color.Red);
    }

    public void Update(Player player)
    {
        FollowPlayer(player);
    }

    public void FollowPlayer(Player player)
    {

        // Räkna ut avståndet mellan spelaren och enemy
        float distance = MathF.Sqrt(MathF.Pow(X - player.X, 2) + MathF.Pow(Y - player.Y, 2));

        // Stannar den är för nära
        if (distance < player.Radius)
            return;

        // Beräkna vinkeln mellan fienden och spelaren.
        float angle = MathF.Atan2(player.Y - Y, player.X - X);

        // Flyttar enemys position baserad på den beräknade vinkeln.
        X += (int)(MathF.Cos(angle) * Speed);
        Y += (int)(MathF.Sin(angle) * Speed);
    }

    public bool BulletCollision(Bullet bullet)
    {
        return MathF.Sqrt((bullet.X - X) * (bullet.X - X) + (bullet.Y - Y) * (bullet.Y - Y)) < Radius + bullet.Radius; // Checkar om enemy nuddar bullet.
    }

    public bool IsDead()
    {
        return Hp <= 0; // Kollar om död
    }
}
