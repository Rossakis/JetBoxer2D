using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.GameObjects.Base;
using Super_Duper_Shooter.Classes.InputSystem;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.GameStates.Base;

/// <summary>
/// Base state of the game that classes like SplashState or GameplayState can inherit
/// </summary>
public abstract class BaseGameState
{
    //All the gameObject present in this GameState
    public List<BaseGameObject> GameObjects { get; set; }

    protected InputManager InputManager { get; set; }
    protected abstract void SetInputManager();

    protected int _viewportWidth;
    protected int _viewportHeight;

    //the name of the texture that will be shown if ContentManager doesn't find the desired texture
    private const string FallbackTexture = "Empty";
    
    public event EventHandler<int> ResolutionChanged;//when resolution changes, switch sprites for animation (e.g. when resolution gets bigger, use 256px sprite and not 128px).
    public void OnResolutionChanged(int resolutionWidth)
    {
        ResolutionChanged?.Invoke(this, resolutionWidth);
    }

    //instead of using ContentManager's Load and Unload methods like singletons, 
    //we will make a local ContentManager for each BaseGameState instance in the game (e.g. GameplayState)
    protected ContentManager _contentManager;
    // protected Game _game; //this game instance
    // protected SpriteBatch _spriteBatch;

    /// <summary>
    /// Assign the MainGame's ContentManager this (local) one
    /// </summary>
    /// <param name="contentManager"></param>
    public virtual void Initialize(ContentManager contentManager, int viewportWidth, int viewportHeight)
    {
        _contentManager = contentManager;
        _viewportWidth = viewportWidth;
        _viewportHeight = viewportHeight;

        GameObjects = new List<BaseGameObject>();
        SetInputManager();
    }

    /// <summary>
    /// Load the local gameObjects
    /// </summary>
    /// <param name="contentManager"></param>
    public abstract void LoadContent(SpriteBatch spriteBatch);

    /// <summary>
    /// Calls the local ContentManager to unload the current game assets 
    /// </summary>
    /// <param name="contentManager"></param>
    public void UnloadContent()
    {
        _contentManager.Unload();
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public abstract void HandleInput(GameTime gameTime);

    //TODO:Both LoadContent() and UnloadContent() should be called though the currentGameState in the MainGame
    protected Texture2D LoadTexture(string textureName)
    {
        var texture = _contentManager.Load<Texture2D>(textureName);

        //If texture variable is null, show the FallbackTexture
        return texture ?? _contentManager.Load<Texture2D>(FallbackTexture);
    }

    public event EventHandler<BaseGameState> OnStateSwitched;
    public event EventHandler<GameEvents> OnEventNotification;

    /// <summary>
    /// Invokes the <see cref="OnStateSwitched"/> event
    /// </summary>
    /// <param name="state"></param>
    public void SwitchState(BaseGameState state)
    {
        OnStateSwitched?.Invoke(this, state);
    }

    /// <summary>
    /// Invokes the <see cref="OnEventNotification"/> event
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="argument">Specifies that if a caller does not provide a value for the "argument" parameter when calling the method, it defaults to "null".</param>
    protected void NotifyEvent(GameEvents eventType, object argument = null)
    {
        OnEventNotification?.Invoke(this, eventType);

        foreach (var gameObject in GameObjects) gameObject.OnNotify(eventType);
    }

    protected void AddGameObject(BaseGameObject gameObject)
    {
        GameObjects.Add(gameObject);
    }

    protected void RemoveGameObject(BaseGameObject gameObject)
    {
        GameObjects?.Remove(gameObject);
    }

    //To be called from the Draw() method
    public void Render(SpriteBatch spriteBatch)
    {
        //render gameObjects (using LINQ query) based on their zIndex, so that they
        //are shown in the correct order
        foreach (var gameObject in GameObjects.OrderBy(gameObj => gameObj.zIndex)) gameObject.Render(spriteBatch);
    }
}