using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Engine.Extensions;
using Super_Duper_Shooter.Engine.Input;
using Super_Duper_Shooter.Engine.States;
using Super_Duper_Shooter.Enums;
using Super_Duper_Shooter.Game.InputMaps;
using Super_Duper_Shooter.Game.Objects;

namespace Super_Duper_Shooter.Game.States;

public class SplashState : BaseGameState
{
    private const string SplashScreen = "Backgrounds/Splash Screen";

    public override void Initialize(Microsoft.Xna.Framework.Game game, ContentManager contentManager, int viewportWidth, int viewportHeight)
    { 
        base.Initialize(game, contentManager, viewportWidth, viewportHeight);
    }

    protected override void SetInputManager()
    {
        InputManager = new InputManager(new SplashInputMap());
    }
    
    public override void Update()
    {
        base.Update();
        InputManager.UpdateInput();

        //If mouse gets moved, show it
        if (MouseInput.GetMouseAxis(MouseInputTypes.Horizontal) != 0 || MouseInput.GetMouseAxis(MouseInputTypes.Vertical) != 0)
            _game.IsMouseVisible = true;
        else
            _game.IsMouseVisible = false;

        //OnSwitch - Switch to Gameplay
        if (InputManager.GetButtonDown(SplashInputMap.EnterGame))
        {
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
        AddGameObject(new SplashImage(LoadTexture(SplashScreen)));
    }
}