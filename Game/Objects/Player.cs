using System;
using System.Collections.Generic;
using JetBoxer2D.Engine.Animation;
using JetBoxer2D.Engine.Extensions;
using JetBoxer2D.Engine.Input;
using JetBoxer2D.Engine.Objects;
using JetBoxer2D.Engine.States;
using JetBoxer2D.Game.Events;
using JetBoxer2D.Game.InputMaps;
using JetBoxer2D.Game.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Game.Objects;

public enum PlayerState
{
    Idle,
    MovingLeft,
    MovingRight,
    ShootLeft,
    ShootRight,
    Death
}

public class Player : BaseGameObject
{
    private readonly SpriteBatch _spriteBatch;
    private readonly ContentManager _contentManager; //to load animation textures
    private PlayerState _currentState;
    private BaseGameState _gameState;

    private AnimationPlayer _animator;

    //The sprite that will be shown by default if no animation is yet set
    private const string DefaultSprite = "Placeholder Texture";

    private const string IdleSprite = "Characters/Jet Boxer - Idle (Rotated)";
    private const string MovingSprite = "Characters/Jet Boxer - Move Right (Rotated)";
    private const string ShootRightSprite = "Characters/Jet Boxer - Shoot Right (Rotated)";
    private const string ShootLeftSprite = "Characters/Jet Boxer - Shoot Left (Rotated)";
    private AnimationClip _idle;
    private AnimationClip _moving;
    private AnimationClip _shootRight;
    private AnimationClip _shootLeft;

    private const float MoveSpeed = 500;
    private readonly InputManager _inputManager;

    public Texture2D Texture => _texture;

    private Vector2 _moveInput; // (horizontal, vertical)
    private Vector2 _lookInput; // (horizontal, vertical)

    private List<Projectile> _projectiles;

    //Make sure player fires only once
    private bool _hasFiredLeft;
    private bool _hasFiredRight;

    //If player tries to shoot right whilst already shooting left (and vice-versa), switch to the 
    //desired shooting state once the animation ends (e.g. Change from ShootingRight to ShootingLeft, once
    //Shooting Right animation ends)
    private bool _fireRightLater;
    private bool _fireLeftLater;

    //Player attack animation acceleration
    private const float StartAnimationSpeed = 1f;
    private const float MaxAnimationSpeed = 12f; //max 10x animation acceleration
    private float _currentAnimationSpeed;
    private readonly float _animationSpeedBuff = 0.5f; //each time player attacks, increment animation speed by this
    private float _lastAttackedTime;
    private const float MaxNonAttackingTime = 0.5f; //if player wasn't attacking for this long, reset animation speed

    private Vector2
        _rotationTarget; //player will rotate towards this target, which simulates a cursor when using a gamepad
    private Vector2 _prevRotationTarget;
    private const float RotationSpeed = 50f;

    public Player(Vector2 playerPos, SpriteBatch spriteBatch, ContentManager contentManager, BaseGameState gameState)
    {
        _position = playerPos;
        _spriteBatch = spriteBatch;
        _contentManager = contentManager;
        _gameState = gameState;
        _inputManager = gameState.InputManager;

        //The texture will define the actual size of the player sprite
        _texture = contentManager.Load<Texture2D>(IdleSprite); //default texture
        Width = Texture.Width;
        Height = Texture.Height;
        _moveInput = Vector2.Zero;
        _animator = new AnimationPlayer(_spriteBatch, this);
        _currentAnimationSpeed = StartAnimationSpeed;
        _projectiles = new List<Projectile>();

        SetAnimations();
        SwitchState(PlayerState.Idle); //default state

        _rotationTarget = Centre;
    }

    private void SetAnimations() //based on the resolution, accordingly sized sprites will be rendered
    {
        _idle = new AnimationClip(_contentManager.Load<Texture2D>(IdleSprite), 96, 80, 0.1f, true);
        _moving = new AnimationClip(_contentManager.Load<Texture2D>(MovingSprite), 96, 80, 0.1f, true);
        _shootRight = new AnimationClip(_contentManager.Load<Texture2D>(ShootRightSprite), 96, 80, 0.1f, false);
        _shootLeft = new AnimationClip(_contentManager.Load<Texture2D>(ShootLeftSprite), 96, 80, 0.1f, false);
    }

    public override void Update()
    {
        //Move - Horizontal Input
        if (_inputManager.GetValue(GameplayInputMap.MoveLeft) != 0)
            _moveInput.X = _inputManager.GetValue(GameplayInputMap.MoveLeft);
        else if (_inputManager.GetValue(GameplayInputMap.MoveRight) != 0)
            _moveInput.X = _inputManager.GetValue(GameplayInputMap.MoveRight);
        else
            _moveInput.X = 0;

        //Move - Vertical Input
        if (_inputManager.GetValue(GameplayInputMap.MoveDown) != 0)
            _moveInput.Y = _inputManager.GetValue(GameplayInputMap.MoveDown);
        else if (_inputManager.GetValue(GameplayInputMap.MoveUp) != 0)
            _moveInput.Y = _inputManager.GetValue(GameplayInputMap.MoveUp);
        else
            _moveInput.Y = 0;

        //Look - Horizontal Input
        if (_inputManager.GetValue(GameplayInputMap.LookLeft) != 0)
            _lookInput.X = _inputManager.GetValue(GameplayInputMap.LookLeft);
        else if (_inputManager.GetValue(GameplayInputMap.LookRight) != 0)
            _lookInput.X = _inputManager.GetValue(GameplayInputMap.LookRight);
        else
            _lookInput.X = 0;

        //Look - Vertical Input
        if (_inputManager.GetValue(GameplayInputMap.LookDown) != 0)
            _lookInput.Y = _inputManager.GetValue(GameplayInputMap.LookDown);
        else if (_inputManager.GetValue(GameplayInputMap.LookUp) != 0)
            _lookInput.Y = _inputManager.GetValue(GameplayInputMap.LookUp);
        else
            _lookInput.Y = 0;

        //Normalize moveInput so that diagonal movement isn't faster than horizontal or vertical
        Vector2.Normalize(_moveInput);

        _position = new Vector2(_position.X + MoveSpeed * Time.DeltaTime * _moveInput.X,
            _position.Y - MoveSpeed * Time.DeltaTime * _moveInput.Y);

        for (var i = 0; i <= _projectiles.Count - 1; i++) _projectiles[i].Update();

        //Animation speed
        if (Time.TotalSeconds - _lastAttackedTime >= MaxNonAttackingTime)
        {
            _currentAnimationSpeed = StartAnimationSpeed;
            _lastAttackedTime = 0f;
        }

        FollowTargetRotation();
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        if (_animator == null)
            throw new Exception($"Animation player wasn't defined");

        switch (_currentState)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.MovingLeft:
                MoveLeft();
                break;
            case PlayerState.MovingRight:
                MoveRight();
                break;
            case PlayerState.ShootLeft:
                ShootLeft();
                break;
            case PlayerState.ShootRight:
                ShootRight();
                break;
        }

        //Select only the projectiles still on the screen
        for (var i = 0; i <= _projectiles.Count - 1; i++)
        {
            _projectiles[i].Render(spriteBatch);

            if (_projectiles[i].Position.Y < -spriteBatch.GraphicsDevice.Viewport.Height
                || _projectiles[i].LifeSpan >= Projectile.MaxLifeSpan)
                // Add the index of the item to be removed
                _projectiles.RemoveAt(i);
        }
    }

    private void SwitchState(PlayerState state)
    {
        if (_animator != null)
            _animator.IsStopped = true;

        _currentState = state;

        if (_animator != null)
            _animator.IsStopped = false;
    }

    private void Idle()
    {
        _animator.Play(_idle);

        if (_moveInput.X < 0)
            SwitchState(PlayerState.MovingLeft);
        if (_moveInput.X > 0)
            SwitchState(PlayerState.MovingRight);

        if (_inputManager.GetButtonDown(GameplayInputMap.ShootLeft))
            SwitchState(PlayerState.ShootLeft);
        if (_inputManager.GetButtonDown(GameplayInputMap.ShootRight))
            SwitchState(PlayerState.ShootRight);
    }

    private void MoveLeft()
    {
        _animator.Play(_moving);

        if (_moveInput == Vector2.Zero) //if not moving
            SwitchState(PlayerState.Idle);

        if (_moveInput.X > 0)
            SwitchState(PlayerState.MovingRight);

        if (_inputManager.GetButtonDown(GameplayInputMap.ShootLeft))
            SwitchState(PlayerState.ShootLeft);
        if (_inputManager.GetButtonDown(GameplayInputMap.ShootRight))
            SwitchState(PlayerState.ShootRight);
    }

    private void MoveRight()
    {
        _animator.Play(_moving);

        if (_moveInput.X == 0)
            SwitchState(PlayerState.Idle);
        else if (_moveInput.X < 0)
            SwitchState(PlayerState.MovingLeft);

        if (_inputManager.GetButtonDown(GameplayInputMap.ShootLeft))
            SwitchState(PlayerState.ShootLeft);
        else if (_inputManager.GetButtonDown(GameplayInputMap.ShootRight))
            SwitchState(PlayerState.ShootRight);
    }

    private void ShootLeft()
    {
        _animator.Play(_shootLeft);
        _animator.IsFlipped = false;
        _animator.AnimationSpeed = _currentAnimationSpeed;

        if (!_hasFiredLeft)
        {
            _projectiles.Add(new Projectile(
                _position, _animator.Rotation,
                _spriteBatch,
                _contentManager, 2));

            _hasFiredLeft = true;

            if (_currentAnimationSpeed < MaxAnimationSpeed)
                _currentAnimationSpeed += _animationSpeedBuff;
            _lastAttackedTime = Time.TotalSeconds;

            _gameState.NotifyEvent(new GameplayEvents.PlayerShoots());
        }

        //if player pressed to fire the other shooting button, switch it once this one ends
        if (!_fireRightLater && _inputManager.GetButtonDown(GameplayInputMap.ShootRight))
            _fireRightLater = true;

        if (_animator.HasEnded) //if the shooting animation ended, return to normal
        {
            if (!_fireRightLater)
            {
                SwitchState(PlayerState.Idle);
            }
            else
            {
                SwitchState(PlayerState.ShootRight);
                _fireRightLater = false;
            }

            _hasFiredLeft = false;
        }
    }

    private void ShootRight()
    {
        _animator.Play(_shootRight);
        _animator.IsFlipped = false;
        _animator.AnimationSpeed = _currentAnimationSpeed;

        if (!_hasFiredRight)
        {
            _projectiles.Add(new Projectile(
                _position, _animator.Rotation,
                _spriteBatch,
                _contentManager, 2));

            _hasFiredRight = true;

            if (_currentAnimationSpeed < MaxAnimationSpeed)
                _currentAnimationSpeed += _animationSpeedBuff;
            _lastAttackedTime = Time.TotalSeconds;

            _gameState.NotifyEvent(new GameplayEvents.PlayerShoots());
        }

        //if player pressed to fire the other shooting button, switch it once this one ends
        if (!_fireLeftLater && _inputManager.GetButtonDown(GameplayInputMap.ShootLeft))
            _fireLeftLater = true;

        if (_animator.HasEnded) //if the shooting animation ended, return to normal
        {
            if (!_fireLeftLater)
            {
                SwitchState(PlayerState.Idle);
            }
            else
            {
                SwitchState(PlayerState.ShootLeft);
                _fireLeftLater = false;
            }

            _hasFiredRight = false;
        }
    }

    private void FollowTargetRotation()
    {
        if (_lookInput.X != 0f) //horizontal
            _rotationTarget.X += _lookInput.X * RotationSpeed;

        if (_lookInput.Y != 0f) //vertical
            _rotationTarget.Y -= _lookInput.Y * RotationSpeed;

        if (_lookInput == Vector2.Zero && _rotationTarget == Vector2.Zero)
            _rotationTarget = Centre;
        else if (_lookInput == Vector2.Zero && _rotationTarget != Vector2.Zero)
            _rotationTarget = _prevRotationTarget;

        _rotationTarget.X = Math.Clamp(_rotationTarget.X, -5000f, _gameState.Graphics.PreferredBackBufferWidth + 5000f);
        _rotationTarget.Y =
            Math.Clamp(_rotationTarget.Y, -5000f, _gameState.Graphics.PreferredBackBufferHeight + 5000f);

        _animator.Rotation = (float) Math.Atan2(_rotationTarget.Y, _rotationTarget.X);

        _prevRotationTarget = _rotationTarget;
    }
}