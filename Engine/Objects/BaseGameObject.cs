﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Engine.Objects;

public class BaseGameObject
{
    protected Texture2D _texture;
    protected Vector2 _position;
    public Vector2 Position { get => _position; set => _position = value; }
    public int Width;
    public int Height;
    public Vector2 Centre => new Vector2(Width / 2f, Height / 2f);
    
    public int zIndex;

    public event EventHandler<GameEvents> Notify;
    public virtual void OnNotify(GameEvents eventType)
    {
        Notify?.Invoke(this, eventType);
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    public virtual void Update()
    {
        
    }
    
    /// <summary>
    /// Render this gameObject's sprites
    /// </summary>
    /// <param name="spriteBatch"></param>
    public virtual void Render(SpriteBatch spriteBatch)
    {
        // TODO: Drawing call here
        if (_texture != null)
            spriteBatch.Draw(_texture, Position, Color.White);
        else
            throw new Exception($"{ToString()} texture field wasn't defined");
    }

    /// <summary>
    /// Get the texture's center point, used when calling the Draw() method
    /// </summary>
    public virtual Vector2 CenterTexture()
    {
        return new Vector2(Width / 2f, Height / 2f);
    }
}