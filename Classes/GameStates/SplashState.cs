using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.GameObjects;
using Super_Duper_Shooter.Classes.GameStates.Base;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.GameStates;

public class SplashState : BaseGameState
{
    private const string SplashScreen = "Backgrounds/Splash Screen";

    public override void HandleInput(GameTime gameTime)
    {
        var gamepadState = GamePad.GetState(PlayerIndex.One);
        var keyboardState = Keyboard.GetState();

        //OnNotify - Exit the game
        if (gamepadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            NotifyEvent(GameEvents.GAME_QUIT);

        //OnSwitch - Switch to Gameplay
        if (keyboardState.IsKeyDown(Keys.Enter))
            SwitchState(new GameplayState());
    }

    protected override void SetInputManager()
    {
        //InputManager = new InputManager(new SplashInputMapper());
    }

    public override void LoadContent(SpriteBatch spriteBatch)
    {
        AddGameObject(new SplashImage(LoadTexture(SplashScreen)));
    }
}