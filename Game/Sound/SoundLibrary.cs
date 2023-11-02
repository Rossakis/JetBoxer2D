using System;
using System.Collections.Generic;

namespace Super_Duper_Shooter.Engine.Sound;

public enum GameSongs
{
    SplashScreen,
    Gameplay1,
    Gameplay2,
    Gameplay3,
    Gameplay4,
    Gameplay5,
    Gameplay6
}

public enum GameSFX
{
    Fireball
}

public static class SoundLibrary
{
    private static Dictionary<GameSongs, string> _gameSongs = new ()
    {
        {GameSongs.SplashScreen, "Sounds/Splash/Splash Screen"},
        
        {GameSongs.Gameplay1, "Sounds/Gameplay/Gameplay Theme - 1"},
        {GameSongs.Gameplay2, "Sounds/Gameplay/Gameplay Theme - 2"},
        {GameSongs.Gameplay3, "Sounds/Gameplay/Gameplay Theme - 3"},
        {GameSongs.Gameplay4, "Sounds/Gameplay/Gameplay Theme - 4"},
        {GameSongs.Gameplay5, "Sounds/Gameplay/Gameplay Theme - 5"},
        {GameSongs.Gameplay6, "Sounds/Gameplay/Gameplay Theme - 6"},
        
    };

    private static Dictionary<GameSFX, string> _gameSFX = new()
    {
        {GameSFX.Fireball, "Sounds/Effects/Fireball"}
    };

    /// <summary>
    /// Returns the first instance of the GameSongs enum in the dictionaries of SoundLibrary
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    public static string GetSong(GameSongs sound)
    {
        try
        {
            foreach (var songDict in _gameSongs)
            {
                if (songDict.Key == sound)
                {
                    return songDict.Value;
                }
            }
            return null;
        }
        catch (NullReferenceException)
        {
            throw new NullReferenceException("No such song was found in the SongLibrary");
        }
        
    }
    
    /// <summary>
    /// Returns the first instance of the GameSFX enum in the dictionaries of SoundLibrary
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    public static string GetSfx(GameSFX sound)
    {
        try
        {
            foreach (var songDict in _gameSFX)
            {
                if (songDict.Key == sound)
                {
                    return songDict.Value;
                }
            }
            return null;
        }
        catch (NullReferenceException)
        {
            throw new NullReferenceException("No such sfx was found in the SongLibrary");
        }
        
    }
}
