using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Engine.Animation;
using Super_Duper_Shooter.Engine.Extensions;
using Super_Duper_Shooter.Engine.Input;
using Super_Duper_Shooter.Engine.Objects;
using Super_Duper_Shooter.Game.Events;
using Super_Duper_Shooter.Game.InputMaps;
using Super_Duper_Shooter.Game.States;

namespace Super_Duper_Shooter.Game.Objects;

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
    private PlayerState CurrentState;

    private AnimationPlayer _animator;

    //The sprite that will be shown by default if no animation is yet set
    private const string DefaultSprite = "Placeholder Texture";

    private const string IdleSprite = "Characters/Jet Boxer (Idle)";
    private const string MovingSprite = "Characters/Jet Boxer (Move Right)";
    private const string ShootingSprite = "Characters/Jet Boxer (Shoot Right)";
    private AnimationClip _idle;
    private AnimationClip _moving;
    private AnimationClip _shooting;

    private const float MoveSpeed = 500;
    private readonly InputManager _inputManager;
    private readonly GameplayState _gameplayState;

    public Texture2D Texture => _texture;

    private Vector2 _moveInput;// (horizontal, vertical)

    private List<Projectile> _projectiles;
    private bool _hasFiredLeft;
    private bool _hasFiredRight;

    //Player attack animation acceleration
    private const float StartAnimationSpeed = 1f;
    private const float MaxAnimationSpeed = 12f;//max 10x animation acceleration
    private float _currentAnimationSpeed;
    private readonly float _animationSpeedBuff = 0.5f;//each time player attacks, increment animation speed by this
    private float _lastAttackedTime;
    private const float MaxNonAttackingTime = 0.5f;//if player wasn't attacking for this long, reset animation speed

    public Player(Vector2 playerPos, SpriteBatch spriteBatch, ContentManager contentManager, GameplayState gameState)
    {
        _position = playerPos;
        _spriteBatch = spriteBatch;
        _contentManager = contentManager;
        _gameplayState = gameState;
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
    }
    
    private void SetAnimations()//based on the resolution, accordingly sized sprites will be rendered
    {
        _idle = new AnimationClip(_contentManager.Load<Texture2D>(IdleSprite), 80, 96, 0.1f, true);
        _moving = new AnimationClip(_contentManager.Load<Texture2D>(MovingSprite), 80, 96, 0.1f, true);
        _shooting = new AnimationClip(_contentManager.Load<Texture2D>(ShootingSprite), 80, 96, 0.1f, false);
    }
    public override void Update()
    {
        //Horizontal Input
        if (_inputManager.GetValue(GameplayInputMap.MoveLeft) != 0)
            _moveInput.X = _inputManager.GetValue(GameplayInputMap.MoveLeft);
        else if(_inputManager.GetValue(GameplayInputMap.MoveRight) != 0)
            _moveInput.X = _inputManager.GetValue(GameplayInputMap.MoveRight);
        else 
            _moveInput.X = 0;
        
        //Vertical Input
        if (_inputManager.GetValue(GameplayInputMap.MoveDown) != 0)
            _moveInput.Y = _inputManager.GetValue(GameplayInputMap.MoveDown);
        else if(_inputManager.GetValue(GameplayInputMap.MoveUp) != 0)
            _moveInput.Y = _inputManager.GetValue(GameplayInputMap.MoveUp);
        else 
            _moveInput.Y = 0;

        //Normalize moveInput so that diagonal movement isn't faster than horizontal or vertical
        Vector2.Normalize(_moveInput);
        
        _position = new Vector2(_position.X + MoveSpeed * Time.DeltaTime * _moveInput.X, _position.Y - MoveSpeed * Time.DeltaTime * _moveInput.Y);
        
        for (int i = 0; i <= _projectiles.Count - 1; i++)
        {
            _projectiles[i].Update();
        }
        
        //Animation speed
        if(Time.TotalSeconds - _lastAttackedTime >= MaxNonAttackingTime)
        {
            _currentAnimationSpeed = StartAnimationSpeed;
            _lastAttackedTime = 0f;
        }

        Console.WriteLine($"Animation Speed: {_currentAnimationSpeed}");
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        if (_animator == null)
            throw new Exception($"Animation player wasn't defined");
        
        switch (CurrentState)
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
        for (int i = 0; i <= _projectiles.Count - 1; i++)
        {
            _projectiles[i].Render(spriteBatch);
            
            if (_projectiles[i].Position.Y < -spriteBatch.GraphicsDevice.Viewport.Height)
            {
                // Add the index of the item to be removed
                Console.WriteLine("Projectile was deleted");
                _projectiles.RemoveAt(i);
            }
        }
    }

    private void SwitchState(PlayerState state)
    {
        if(_animator != null)
            _animator.IsStopped = true;
        
        CurrentState = state;

        if(_animator != null)
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
        if(_inputManager.GetButtonDown(GameplayInputMap.ShootRight))
            SwitchState(PlayerState.ShootRight);

    }

    private void MoveLeft()
    {
        _animator.Play(_moving);
        _animator.IsFlipped = true;

        if (_moveInput == Vector2.Zero)//if not moving
            SwitchState(PlayerState.Idle);
        
        if (_moveInput.X > 0)
            SwitchState(PlayerState.MovingRight);
        
        if (_inputManager.GetButtonDown(GameplayInputMap.ShootLeft))
            SwitchState(PlayerState.ShootLeft);
        if(_inputManager.GetButtonDown(GameplayInputMap.ShootRight))
            SwitchState(PlayerState.ShootRight);
    }

    private void MoveRight()
    {
        _animator.Play(_moving);
        _animator.IsFlipped = false;

        if (_moveInput.X == 0)
            SwitchState(PlayerState.Idle);
        
        if (_moveInput.X < 0)
            SwitchState(PlayerState.MovingLeft);
        
        if (_inputManager.GetButtonDown(GameplayInputMap.ShootLeft))
            SwitchState(PlayerState.ShootLeft);
        if(_inputManager.GetButtonDown(GameplayInputMap.ShootRight))
            SwitchState(PlayerState.ShootRight);    
    }

    private void ShootLeft()
    {
        _animator.Play(_shooting);
        _animator.IsFlipped = true;
        _animator.AnimationSpeed = _currentAnimationSpeed; 
        
        if(!_hasFiredLeft)
        {
            _projectiles.Add(new Projectile(
            new Vector2(Position.X - 25, Position.Y - _texture.Height / 2f), 
            _spriteBatch,
            _contentManager, 2));

            _hasFiredLeft = true;
            
            if(_currentAnimationSpeed < MaxAnimationSpeed)
                _currentAnimationSpeed += _animationSpeedBuff;
            _lastAttackedTime = Time.TotalSeconds;
            
            _gameplayState.NotifyEvent(new GameplayEvents.PlayerShoots());
        }

        if (_animator.HasEnded) //if the shooting animation ended, return to normal
        {
            SwitchState(PlayerState.Idle);
            _hasFiredLeft = false;
        }
    }
    
    private void ShootRight()
    {
        _animator.Play(_shooting);
        _animator.IsFlipped = false;
        _animator.AnimationSpeed = _currentAnimationSpeed;

        if (!_hasFiredRight)
        {
            _projectiles.Add(new Projectile(
                new Vector2(Position.X + 25, Position.Y - _texture.Height / 2f),
                _spriteBatch,
                _contentManager, 2));
            
            _hasFiredRight = true;
            
            if(_currentAnimationSpeed < MaxAnimationSpeed)
                _currentAnimationSpeed += _animationSpeedBuff;
            _lastAttackedTime = Time.TotalSeconds;
            
            _gameplayState.NotifyEvent(new GameplayEvents.PlayerShoots());
        }
        
        if (_animator.HasEnded) //if the shooting animation ended, return to normal
        {
            SwitchState(PlayerState.Idle);
            _hasFiredRight = false;
        }
    }
}