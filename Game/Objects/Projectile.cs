using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Engine.Animation;
using Super_Duper_Shooter.Engine.Extensions;
using Super_Duper_Shooter.Engine.Objects;

namespace Super_Duper_Shooter.Game.Objects;

public class Projectile : BaseGameObject
{
    private const float MoveSpeed = 350;
    private const string DefaultSprite = "Placeholder Texture";
    private const string MoveAnimation = "Effects/Energy Blast";
    
    private AnimationPlayer _animationPlayer;
    private AnimationClip _moveAnimation;
    
    public Projectile(Vector2 position, SpriteBatch spriteBatch, ContentManager contentManager, int zIndex)
    {
        _texture = contentManager.Load<Texture2D>(DefaultSprite);
        Position = position;
        this.zIndex = zIndex;
        
        _animationPlayer = new AnimationPlayer(spriteBatch, this);
        _moveAnimation = new AnimationClip(contentManager.Load<Texture2D>(MoveAnimation), 0.1f, true);
    }

    public void MoveUp()
    {
        _animationPlayer.Play(_moveAnimation);
        Position = new Vector2(Position.X, Position.Y - MoveSpeed * Time.DeltaTime);
    }
}