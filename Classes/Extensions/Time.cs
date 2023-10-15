using System;
using Microsoft.Xna.Framework;

namespace Super_Duper_Shooter.Classes.Extensions;

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
                throw new Exception("Time.Instance wasn't instantiated");

            return _instance;
        }
        set => _instance = value;
    }

    public static float DeltaTime { get; private set; }

    public static void Update(GameTime gameTime)
    {
        DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
    }
}