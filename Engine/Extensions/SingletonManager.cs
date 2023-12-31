using Microsoft.Xna.Framework;

namespace JetBoxer2D.Engine.Extensions;

/// <summary>
/// Initialize all the singletons from here for code readability
/// </summary>
public static class SingletonManager
{
    public static void InitializeSingletons()
    {
        MouseInput.Instance = new MouseInput();
        Time.Instance = new Time();
    }

    public static void UpdateSingletons(GameTime gameTime)
    {
        MouseInput.Update();
        Time.Update(gameTime);
    }
}