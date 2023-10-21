using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.GameObjects;
using Super_Duper_Shooter.Classes.GameStates.Base;
using Super_Duper_Shooter.Classes.InputSystem;
using Super_Duper_Shooter.Classes.InputSystem.InputMaps;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.GameStates;

public class SplashState : BaseGameState
{
    private const string SplashScreen = "Backgrounds/Splash Screen";
    
    protected override void SetInputManager()
    {
        InputManager = new InputManager(new SplashInputMap());
    }
    
    public override void HandleInput(GameTime gameTime)
    {
        InputManager.UpdateInput();
        
        //OnNotify - Exit the game
        if (InputManager.GetButtonDown(SplashInputMap.ExitGame.InputAction))
        {
            NotifyEvent(GameEvents.GAME_QUIT);
        }
        
        //OnSwitch - Switch to Gameplay
        if (InputManager.GetButtonDown(SplashInputMap.EnterGame.InputAction))
        {
            SwitchState(new GameplayState());
        }
       
    }

    public override void LoadContent(SpriteBatch spriteBatch)
    {
        AddGameObject(new SplashImage(LoadTexture(SplashScreen)));
    }
}