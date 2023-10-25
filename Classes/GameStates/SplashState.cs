using System;
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
    private int count;
    protected override void SetInputManager()
    {
        InputManager = new InputManager(new SplashInputMap());
        count = 0;
    }
    
    public override void HandleInput(GameTime gameTime)
    {
        InputManager.UpdateInput();

        //OnSwitch - Switch to Gameplay
        if (InputManager.GetButtonDown(SplashInputMap.EnterGame))
        {
            // count++;
            // Console.WriteLine($"Enter was pressed {count} times");
            SwitchState(new GameplayState());
        }
        
        //OnNotify - Exit the game
        if (InputManager.GetButtonDown(SplashInputMap.ExitGame))
        {
            NotifyEvent(GameEvents.GameQuit);
        }
       
    }

    public override void LoadContent(SpriteBatch spriteBatch)
    {
        if (_viewportWidth <= 720)
        {
            
        }
        AddGameObject(new SplashImage(LoadTexture(SplashScreen)));
    }
}