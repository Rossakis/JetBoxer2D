﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Engine.Input;
using Super_Duper_Shooter.Engine.States;
using Super_Duper_Shooter.Game.InputMaps;
using Super_Duper_Shooter.Game.Objects;

namespace Super_Duper_Shooter.Game.States;

public class GameplayState : BaseGameState
{
    private const string BackgroundTexture = "Backgrounds/Barren";

    private TerrainBackground _terrainBackground;
    private Player _player;

    public override void Initialize(Microsoft.Xna.Framework.Game game, ContentManager contentManager, int viewportWidth, int viewportHeight)
    {
        base.Initialize(game, contentManager, viewportWidth, viewportHeight);
        game.IsMouseVisible = false;
    }

    protected override void SetInputManager()
    {
        InputManager = new InputManager(new GameplayInputMap());
    }
    
    public override void Update()
    {
        base.Update();
        InputManager.UpdateInput();
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        base.Render(spriteBatch);
        KeepPlayerInBounds();
    }

    public override void LoadContent(SpriteBatch spriteBatch)
    {
        _player = new Player(Vector2.Zero, spriteBatch, _contentManager, InputManager)
            {zIndex = 1};
        _player.Position = new Vector2(_viewportWidth / 2f - _player.Texture.Width,
            _viewportHeight / 2f - _player.Texture.Height);
        AddGameObject(_player);

        //Set the background's depth layer
        _terrainBackground = new TerrainBackground(LoadTexture(BackgroundTexture))
            {zIndex = 0}; //Object Initialization Pattern!
        AddGameObject(_terrainBackground);
    }

    private void KeepPlayerInBounds()
    {
        //We subtract from the player position either "player.Texture.Width" or "player.Texture.Height" due to the player sprite being centered
        
        //Left Side
        if (_player.Position.X - _player.Width/2f < 0)
            _player.Position = new Vector2(_player.Width/2f, _player.Position.Y);

        //Right side
        if (_player.Position.X  > _viewportWidth - _player.Width/2f)
            _player.Position = new Vector2(_viewportWidth - _player.Width/2f, _player.Position.Y);

        //Down Side
        if (_player.Position.Y - _player.Height/2f < 0)
            _player.Position = new Vector2(_player.Position.X, _player.Height/2f);

        //Up side
        if (_player.Position.Y > _viewportHeight - _player.Height/2f)
            _player.Position = new Vector2(_player.Position.X, _viewportHeight - _player.Height/2f);
    }
}