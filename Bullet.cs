using Raylib_cs;
using System.Numerics;

class Bullet
{
    public int Speed = 50;
    public float X { get; set; }
    public float Y { get; set; }
    public float Angle { get; set; }

    public Bullet(float startX, float startY, float angle)
    {
        X = startX;
        Y = startY;
        Angle = angle;
    }

    public void Draw()
    {
        Raylib.DrawCircle((int)X, (int)Y, 8, Color.Yellow);
    }

    public void Update()
    {
        X += Speed * MathF.Cos(Angle * MathF.PI / 180);
        Y += Speed * MathF.Sin(Angle * MathF.PI / 180);
    }
}