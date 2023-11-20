using System.Collections.Generic;
using JetBoxer2D.Engine.Events;
using JetBoxer2D.Engine.Input;
using JetBoxer2D.Engine.States;
using JetBoxer2D.Game.Events;
using JetBoxer2D.Game.InputMaps;
using JetBoxer2D.Game.Objects;
using JetBoxer2D.Game.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Game.States;

public class GameplayState : BaseGameState
{
    private const string BackgroundTexture = "Backgrounds/Barren";

    private TerrainBackground _terrainBackground;
    private Player _player;
    private RedTriangle _redTriangle;

    public override void Initialize(Microsoft.Xna.Framework.Game game, ContentManager contentManager, int viewportWidth,
        int viewportHeight)
    {
        base.Initialize(game, contentManager, viewportWidth, viewportHeight);
        game.IsMouseVisible = true;
    }

    protected override void SetInputManager()
    {
        InputManager = new InputManager(new GameplayInputMap());
    }

    protected override void SetSoundtrack()
    {
        //Sound
        var gameplaySong1 = LoadSound(SoundLibrary.GetSong(GameSongs.Gameplay1)).CreateInstance();
        var gameplaySong2 = LoadSound(SoundLibrary.GetSong(GameSongs.Gameplay2)).CreateInstance();
        var gameplaySong3 = LoadSound(SoundLibrary.GetSong(GameSongs.Gameplay3)).CreateInstance();
        var gameplaySong4 = LoadSound(SoundLibrary.GetSong(GameSongs.Gameplay4)).CreateInstance();
        var gameplaySong5 = LoadSound(SoundLibrary.GetSong(GameSongs.Gameplay5)).CreateInstance();
        var gameplaySong6 = LoadSound(SoundLibrary.GetSong(GameSongs.Gameplay6)).CreateInstance();

        SoundManager.SetSoundtrack(new List<SoundEffectInstance>
        {
            gameplaySong1,
            gameplaySong2,
            gameplaySong3,
            gameplaySong4,
            gameplaySong5,
            gameplaySong6
        });

        var fireballSound = LoadSound(SoundLibrary.GetSfx(GameSFX.Fireball));
        SoundManager.RegisterSound(new GameplayEvents.PlayerShoots(), fireballSound);
        _isSoundInitialized = true;
    }

    protected override void UpdateGameState()
    {
        //OnNotify - Exit the game
        if (InputManager.GetButtonDown(SplashInputMap.ExitGame))
            //Beware, if you switch to this Game State without first initializing SplashInputMap, it will crash
            NotifyEvent(new BaseGameStateEvent.GameQuit());

        _redTriangle?.SetPlayerPosition(_player.Position);
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        base.Render(spriteBatch);

        KeepPlayerInBounds();
    }

    public override void LoadContent(SpriteBatch spriteBatch)
    {
        //Sound
        SetSoundtrack();

        _player = new Player(Vector2.Zero, spriteBatch, _contentManager, this)
            {zIndex = 1};

        _player.Position = new Vector2(_viewportWidth / 2f - _player.Texture.Width,
            _viewportHeight / 2f - _player.Texture.Height);


        AddGameObject(_player);

        _redTriangle = new RedTriangle(_contentManager, spriteBatch, _player)
            {zIndex = 1};
        _redTriangle.Position = Vector2.Zero;

        AddGameObject(_redTriangle);

        //Set the background's depth layer
        _terrainBackground = new TerrainBackground(LoadTexture(BackgroundTexture))
            {zIndex = 0}; //Object Initialization Pattern!
        AddGameObject(_terrainBackground);
    }

    private void KeepPlayerInBounds()
    {
        //Left Side
        if (_player.Position.X - _player.Width / 2f < 0)
            _player.Position = new Vector2(_player.Width / 2f, _player.Position.Y);

        //Right side
        if (_player.Position.X > _viewportWidth - _player.Width / 2f)
            _player.Position = new Vector2(_viewportWidth - _player.Width / 2f, _player.Position.Y);

        //Down Side
        if (_player.Position.Y - _player.Height / 2f < 0)
            _player.Position = new Vector2(_player.Position.X, _player.Height / 2f);

        //Up side
        if (_player.Position.Y > _viewportHeight - _player.Height / 2f)
            _player.Position = new Vector2(_player.Position.X, _viewportHeight - _player.Height / 2f);
    }
}