using Raylib_cs;


class Player{
    public int X {get; set;} = Raylib.GetScreenWidth() / 2;
    public int Y {get; set;} = Raylib.GetScreenHeight() / 2;
    public int Radius {get;set;} = 20;
    public Raylib_cs.Color Color {get;set;} = Color.Blue;
    
    public int Hp {get; set;} = 100;
    public int Speed {get; set;} = 5;
    public int FireRate {get; set;} = 5;
    public int Damage {get; set;} = 20;


    public void Update(){
        Movement();
    }

    public void Draw() {
        Raylib.DrawCircle(X, Y,Radius, Color);
    }

    void Movement(){
        if(Raylib.IsKeyDown(KeyboardKey.W)) Y -= Speed;
        if(Raylib.IsKeyDown(KeyboardKey.S)) Y += Speed;
        if(Raylib.IsKeyDown(KeyboardKey.D)) X += Speed;
        if(Raylib.IsKeyDown(KeyboardKey.A)) X -= Speed;
    }
}