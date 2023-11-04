using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Engine.Animation;
using Super_Duper_Shooter.Engine.Extensions;
using Super_Duper_Shooter.Engine.Objects;
using Super_Duper_Shooter.Game.Particles;

namespace Super_Duper_Shooter.Game.Objects;

public class Projectile : BaseGameObject
{
    //private const float MoveSpeed = 350;
    private const float Acceleration = 20f;

    private const string DefaultSprite = "Placeholder Texture";
    private const string MoveAnimation = "Effects/Energy Blast";
    private const string FlameSpark = "Effects/Spark";

    private float currentSpeed = 0;

    public override Vector2 Position
    {
        set
        {
            _position = value;
            _flameEmitter.Position = new Vector2(_position.X - 6, _position.Y + 18);
        }
    }

    private AnimationPlayer _animationPlayer;
    private AnimationClip _moveAnimation;
    
    private FlameSparksEmitter _flameEmitter;
    
    public Projectile(Vector2 position, SpriteBatch spriteBatch, ContentManager contentManager, int zIndex)
    {
        _texture = contentManager.Load<Texture2D>(DefaultSprite);
        _position = position;
        this.zIndex = zIndex;
        
        _animationPlayer = new AnimationPlayer(spriteBatch, this);
        _moveAnimation = new AnimationClip(contentManager.Load<Texture2D>(MoveAnimation), 0.1f, true);

        _flameEmitter = new FlameSparksEmitter(contentManager.Load<Texture2D>(FlameSpark), _position);
    }
    

    public override void Update()
    {
        _flameEmitter.Update();
        
        Position = new Vector2(Position.X, Position.Y - currentSpeed * Time.DeltaTime);
        currentSpeed += Acceleration;
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        _flameEmitter.Render(spriteBatch);

        _animationPlayer.Play(_moveAnimation);
    }
}