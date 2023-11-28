using System;
using JetBoxer2D.Engine.Animation;
using JetBoxer2D.Engine.Events;
using JetBoxer2D.Engine.Extensions;
using JetBoxer2D.Engine.Interfaces;
using JetBoxer2D.Engine.Objects;
using JetBoxer2D.Game.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Game.Objects;

public class RedTriangle : BaseGameObject
{
    private const string
        DefaultTextureSprite =
            "Characters/Enemy/Red Triangle-Default Texture"; //a single frame that defines this object's texture width and height

    private const string IdleSprite = "Characters/Enemy/Red Triangle";

    private AnimationClip _idle;
    private AnimationPlayer _animator;

    private float _rotation;
    private float _direction;

    private const int InitialVelocity = 200;
    private const int Acceleration = 3;
    private float _currentVelocity = InitialVelocity;

    private Vector2 _targetDist;
    private Vector2 _targetDir;

    //if enemy is at minimum Distance the player, stop following him and just follow the previously established direction
    private const int MinDistance = 125;
    private bool _isCloseEnough;

    private const float
        DriftTime = 0.75f; //if player dodges the enemy, how long the enemy will continue to drift in previous direction

    private float _timer;

    // enemy will flash white when hit
    private int _hitAt = 0;
    private int _life = 100;


    public RedTriangle(ContentManager contentManager, SpriteBatch spriteBatch, Player playerTarget)
    {
        _texture = contentManager.Load<Texture2D>(DefaultTextureSprite);
        Width = _texture.Width;
        Height = _texture.Height;

        _animator = new AnimationPlayer(spriteBatch, this);
        _idle = new AnimationClip(contentManager.Load<Texture2D>(IdleSprite), Width, Height, 0.1f, true);
        _timer = 0;
    }

    public override void Update()
    {
        FollowPlayer();
        _currentVelocity += Acceleration;
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        _animator.Play(_idle);
        _animator.Rotation = (float) Math.Atan2(_targetDir.Y, _targetDir.X);
    }

    private void FollowPlayer()
    {
        if (_targetDir.Y > 0) //Player is lower than the enemy
            _position.Y += _targetDir.Y * _currentVelocity * Time.DeltaTime;
        else if (_targetDir.Y < 0) //Player is higher
            _position.Y -= _targetDir.Y * -_currentVelocity * Time.DeltaTime;

        if (_targetDir.X > 0) //Player is to the right
            _position.X += _targetDir.X * _currentVelocity * Time.DeltaTime;
        else if (_targetDir.X < 0) //Player is to the left
            _position.X -= _targetDir.X * -_currentVelocity * Time.DeltaTime;
    }

    //Called by the current Game State
    public void SetPlayerPosition(Vector2 targetPos)
    {
        if (!_isCloseEnough)
            _targetDist = targetPos - _position;

        //after enemy gets close enough, stop following player 
        if (_targetDist.Length() <= MinDistance && !_isCloseEnough)
            _isCloseEnough = true;

        if (_isCloseEnough) //after enemy has approached player
        {
            //count time while drifting
            _timer += Time.DeltaTime;

            if (_timer >= DriftTime) //when drift time ends
            {
                _isCloseEnough = false;
                _timer = 0f;
                _currentVelocity = InitialVelocity; //reset speed
            }
        }

        _targetDir = Vector2.Normalize(_targetDist);
    }

    private void JustHit(IDamageable o)
    {
        _hitAt = 0;
        _life -= o.Damage;
    }

    public override void OnNotify(BaseGameStateEvent gameEvent)
    {
        switch (gameEvent)
        {
            case GameplayEvents.EnemyHitBy m:
                JustHit(m.HitBy);
                SendEvent(new GameplayEvents.EnemyLostLife(_life));
                break;
        }
    }
}