using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.Extensions.ContentGeneration;
using Super_Duper_Shooter.Classes.GameObjects.Base;

namespace Super_Duper_Shooter.Classes.GameObjects;

public class Projectile : BaseGameObject
{
    private const float MoveSpeed = 350;
    private const string DefaultSprite = "Placeholder Texture";
    private const string MoveAnimation = "Effects/Energy Blast";
    
    private AnimationPlayer _animationPlayer;
    private Animation _moveAnimation;

    private TimeSpan _lastShotAt;

    public Projectile(Vector2 position, SpriteBatch spriteBatch, ContentManager contentManager, int zIndex)
    {
        _texture = contentManager.Load<Texture2D>(DefaultSprite);
        Position = position;
        this.zIndex = zIndex;
        
        _animationPlayer = new AnimationPlayer(spriteBatch, this);
        _moveAnimation = new Animation(contentManager.Load<Texture2D>(MoveAnimation), 0.1f, true);
    }

    public void MoveUp()
    {
        _animationPlayer.Play(_moveAnimation);
        Position = new Vector2(Position.X, Position.Y - MoveSpeed * Time.DeltaTime);
    }
}