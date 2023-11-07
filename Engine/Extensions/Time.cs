using System;
using Microsoft.Xna.Framework;

namespace JetBoxer2D.Engine.Extensions;

/// <summary>
/// A singleton oriented class that gives to the DeltaTime of the game to other classes
/// </summary>
public class Time
{
    private static Time _instance;

    public static Time Instance
    {
        get //if instance = null, create new
        {
            if (_instance == null)
                _instance = new Time();

            return _instance;
        }
        set => _instance = value;
    }

    public static float DeltaTime { get; private set; }
    public static TimeSpan GameTime { get; private set; }
    public static float TotalSeconds => (float)GameTime.TotalSeconds;

    public static void Update(GameTime gameTime)
    {
        DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        GameTime = gameTime.TotalGameTime;
    }
}