using MongoDB.Bson.IO;
using Raylib_cs;

class Program
{
    static void Main()
    {

        Raylib.InitWindow(800,600, "Sigma game");
        Player p = new();

        while(!Raylib.WindowShouldClose()){
            p.Update();

            Raylib.SetTargetFPS(60);
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Green);
            p.Draw();
            Raylib.EndDrawing();

        }


    }
}