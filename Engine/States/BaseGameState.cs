using System;
using System.Collections.Generic;
using System.Linq;
using JetBoxer2D.Engine.Events;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using JetBoxer2D.Engine.Input;
using JetBoxer2D.Engine.Objects;
using JetBoxer2D.Engine.Sound;

namespace JetBoxer2D.Engine.States;

/// <summary>
/// Base state of the game that classes like SplashState or GameplayState can inherit
/// </summary>
public abstract class BaseGameState
{
    //All the BaseGameObjects present in this GameState
    private readonly List<BaseGameObject> _gameObjects = new();
    protected SoundManager SoundManager = new();
    public InputManager InputManager { get; set; }
    
    protected int _viewportWidth;
    protected int _viewportHeight;

    protected bool _isSoundInitialized; 
    
    protected abstract void SetInputManager();
    /// <summary>
    /// To be loaded in the LoadContent() method
    /// </summary>
    protected abstract void SetSoundtrack();
    protected abstract void UpdateGameState();
    
   
    //the name of the texture that will be shown if ContentManager doesn't find the desired texture
    private const string FallbackTexture = "Empty";
    

    //instead of using ContentManager's Load and Unload methods like singletons, 
    //we will make a local ContentManager for each BaseGameState instance in the game (e.g. GameplayState)
    protected ContentManager _contentManager;
    protected Microsoft.Xna.Framework.Game _game; //this game instance

    /// <summary>
    /// Assign the MainGame's ContentManager this (local) one
    /// </summary>
    /// <param name="contentManager"></param>
    public virtual void Initialize(Microsoft.Xna.Framework.Game game, ContentManager contentManager, int viewportWidth, int viewportHeight)
    {
        _game = game;
        _contentManager = contentManager;
        _viewportWidth = viewportWidth;
        _viewportHeight = viewportHeight;

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
        _isSoundInitialized = false;
    }
    
    //TODO:Both LoadContent() and UnloadContent() should be called though the currentGameState in the MainGame
    protected Texture2D LoadTexture(string textureName)
    {
        var texture = _contentManager.Load<Texture2D>(textureName);

        //If texture variable is null, show the FallbackTexture
        return texture ?? _contentManager.Load<Texture2D>(FallbackTexture);
    }

    public event EventHandler<BaseGameState> OnStateSwitched;
    public event EventHandler<BaseGameStateEvent> OnEventNotification;

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
    public void NotifyEvent(BaseGameStateEvent eventType, object argument = null)
    {
        OnEventNotification?.Invoke(this, eventType);

        foreach (var gameObject in _gameObjects) 
            gameObject.OnNotify(eventType);
        
        SoundManager.OnNotify(eventType);
    }

    protected void AddGameObject(BaseGameObject gameObject)
    {
        _gameObjects.Add(gameObject);
    }

    protected void RemoveGameObject(BaseGameObject gameObject)
    {
        _gameObjects?.Remove(gameObject);
    }

    protected SoundEffect LoadSound(string soundName)
    {
        return _contentManager.Load<SoundEffect>(soundName);
    }
    
    public void Update()
    {
        UpdateGameState();
        InputManager.UpdateInput();
        
        if(_isSoundInitialized)
            SoundManager.PlaySoundtrack();
        
        foreach (var gameObject in _gameObjects.OrderBy(gameObj => gameObj.zIndex)) 
            gameObject.Update();
    }
    
    //To be called from the Draw() method
    public virtual void Render(SpriteBatch spriteBatch)
    {
        //render gameObjects (using LINQ query) based on their zIndex, so that they
        //are shown in the correct order
        foreach (var gameObject in _gameObjects.OrderBy(gameObj => gameObj.zIndex)) 
            gameObject.Render(spriteBatch);
    }
}