using Raylib_cs;

class Program
{
    static void Main()
    {

        Raylib.InitWindow(1200,800, "Sigma game");
        Player p = new();
        Bush[] bushes = new Bush[5];
        for (int i = 0; i < bushes.Length; i++)
        {
            bushes[i] = new Bush();
        }


        while(!Raylib.WindowShouldClose()){
            p.Update();

            Raylib.SetTargetFPS(60);
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Green);
            p.Draw();
            foreach(Bush b in bushes)
                b.Draw();
            Raylib.EndDrawing();

        }
        Raylib.UnloadTexture(p.texture);
        Raylib.CloseWindow();


    }
}


class Bush{
    public int Radius = Random.Shared.Next(10, 50);
    public int X = Random.Shared.Next(Raylib.GetScreenWidth());
    public int Y = Random.Shared.Next(Raylib.GetScreenHeight());

    public void Draw(){
        Raylib.DrawCircle(X,Y, Radius, Color.DarkGreen);
    }
}