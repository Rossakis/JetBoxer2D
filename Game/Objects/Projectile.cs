using JetBoxer2D.Engine.Animation;
using JetBoxer2D.Engine.Extensions;
using JetBoxer2D.Engine.Objects;
using JetBoxer2D.Game.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Game.Objects;

public class Projectile : BaseGameObject
{
    private const float Acceleration = 20f;

    private const string DefaultSprite = "Placeholder Texture";
    private const string MoveAnimation = "Effects/Energy Blast";
    private const string FlameSpark = "Effects/Spark";

    private float _currentSpeed;

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
        _currentSpeed = 0;
        
        _animationPlayer = new AnimationPlayer(spriteBatch, this);
        _moveAnimation = new AnimationClip(contentManager.Load<Texture2D>(MoveAnimation), 0.1f, true);

        _flameEmitter = new FlameSparksEmitter(contentManager.Load<Texture2D>(FlameSpark), _position);
    }
    

    public override void Update()
    {
        _flameEmitter.Update();
        
        Position = new Vector2(Position.X, Position.Y - _currentSpeed * Time.DeltaTime);
        _currentSpeed += Acceleration;
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        _flameEmitter.Render(spriteBatch);

        _animationPlayer.Play(_moveAnimation);
    }
}