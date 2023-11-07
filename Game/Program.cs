using JetBoxer2D.Engine;
using JetBoxer2D.Game.States;

//Window resolution in pixels
const int Width = 1280;
const int Height = 720;

using var game = new MainGame(Width, Height, new SplashState());
game.Run();