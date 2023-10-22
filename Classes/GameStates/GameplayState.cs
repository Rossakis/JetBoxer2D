using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Super_Duper_Shooter.Classes.GameObjects;
using Super_Duper_Shooter.Classes.GameStates.Base;
using Super_Duper_Shooter.Classes.InputSystem;
using Super_Duper_Shooter.Classes.InputSystem.InputMaps;

namespace Super_Duper_Shooter.Classes.GameStates;

public class GameplayState : BaseGameState
{
    private const string BackgroundTexture = "Backgrounds/Barren";

    private TerrainBackground _terrainBackground;
    private Player _player;
    private SpriteBatch _spriteBatch;

    protected override void SetInputManager()
    {
        InputManager = new InputManager(new GameplayInputMap());
    }
    
    public override void HandleInput(GameTime gameTime)
    {
        InputManager.UpdateInput();
        KeepPlayerInBounds();
    }


    public override void LoadContent(SpriteBatch spriteBatch)
    {
        _player = new Player(Vector2.Zero, spriteBatch, _contentManager, InputManager)
            {zIndex = 1};
        _player.PlayerPos = new Vector2(_viewportWidth / 2f - _player.Texture.Height,
            _viewportHeight / 2f - _player.Texture.Height);
        AddGameObject(_player);

        //Set the background's depth layer
        _terrainBackground = new TerrainBackground(LoadTexture(BackgroundTexture))
            {zIndex = 0}; //Object Initialization Pattern!
        AddGameObject(_terrainBackground);
    }

    private void KeepPlayerInBounds()
    {
        if (_player.PlayerPos.X < 0) //Below zero means that player exits the left side of the screen
            _player.PlayerPos = new Vector2(0, _player.PlayerPos.Y);

        if (_player.PlayerPos.X >
            _viewportWidth -
            _player.Texture.Height) //Player Width and Height are the same of the 64x64 pixels player sprite
            _player.PlayerPos = new Vector2(_viewportWidth - _player.Texture.Height, _player.PlayerPos.Y);

        if (_player.PlayerPos.Y < 0) //Below zero means that player exits the down side of the screen
            _player.PlayerPos = new Vector2(_player.PlayerPos.X, 0);

        if (_player.PlayerPos.Y > _viewportHeight - _player.Texture.Height)
            _player.PlayerPos = new Vector2(_player.PlayerPos.X, _viewportHeight - _player.Texture.Height);
    }
}