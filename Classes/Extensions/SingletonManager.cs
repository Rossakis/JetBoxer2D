using Microsoft.Xna.Framework;
using Super_Duper_Shooter.Classes.InputEvents;

namespace Super_Duper_Shooter.Classes.Extensions;

/// <summary>
/// Initialize all the singletons from here for code readability
/// </summary>
public static class SingletonManager
{
    public static void InitializeSingletons()
    {
        MouseInput.Instance = new MouseInput();
        Time.Instance = new Time();
        InputManager.Instance = new InputManager();
    }

    public static void UpdateSingletons(GameTime gameTime)
    {
        MouseInput.Update();
        Time.Update(gameTime);
    }
}