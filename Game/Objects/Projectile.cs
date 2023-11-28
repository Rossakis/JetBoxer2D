using System;
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
    private Vector2 _direction;
    private float _rotation;
    public float LifeSpan { get; set; }
    public const float MaxLifeSpan = 3.5f;

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

    public Projectile(Vector2 position, float rotation, SpriteBatch spriteBatch, ContentManager contentManager,
        int zIndex)
    {
        _texture = contentManager.Load<Texture2D>(DefaultSprite);
        _position = position;
        _rotation = rotation;
        _direction = new Vector2((float) Math.Cos(rotation), (float) Math.Sin(rotation));
        this.zIndex = zIndex;
        _currentSpeed = 0;

        _animationPlayer = new AnimationPlayer(spriteBatch, this);
        _moveAnimation = new AnimationClip(contentManager.Load<Texture2D>(MoveAnimation), 0.1f, true);
        _animationPlayer.Rotation = _rotation;

        _flameEmitter = new FlameSparksEmitter(contentManager.Load<Texture2D>(FlameSpark), _position, _rotation);
    }


    public override void Update()
    {
        _flameEmitter.Update();

        Position += _currentSpeed * Time.DeltaTime * _direction;
        _currentSpeed += Acceleration;
        LifeSpan += Time.DeltaTime;
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        _flameEmitter.Render(spriteBatch);

        _animationPlayer.Play(_moveAnimation);
    }
}