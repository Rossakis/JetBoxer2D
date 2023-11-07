using System.Collections.Generic;
using JetBoxer2D.Engine.Events;
using JetBoxer2D.Engine.Extensions;
using JetBoxer2D.Engine.Input;
using JetBoxer2D.Engine.Sound;
using JetBoxer2D.Engine.States;
using JetBoxer2D.Game.InputMaps;
using JetBoxer2D.Game.Objects;
using JetBoxer2D.Game.Sound;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Game.States;

public class SplashState : BaseGameState
{
    private const string SplashScreen = "Backgrounds/Splash Screen";
    
    protected override void SetInputManager()
    {
        InputManager = new InputManager(new SplashInputMap());
    }
    
    protected override void SetSoundtrack()
    {
        //Sound
        var splashSong = LoadSound(SoundLibrary.GetSong(GameSongs.SplashScreen)).CreateInstance();
        SoundManager.SetSoundtrack(new List<SoundEffectInstance>{splashSong});
        
        _isSoundInitialized = true;
    }

    protected override void UpdateGameState()
    {
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
            NotifyEvent(new BaseGameStateEvent.GameQuit());
        }
       
    }

    public override void LoadContent(SpriteBatch spriteBatch)
    {
        SetSoundtrack();

        AddGameObject(new SplashImage(LoadTexture(SplashScreen)));
    }
}