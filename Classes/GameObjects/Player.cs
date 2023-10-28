using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.Extensions.ContentGeneration;
using Super_Duper_Shooter.Classes.GameObjects.Base;
using Super_Duper_Shooter.Classes.InputSystem;
using Super_Duper_Shooter.Classes.InputSystem.InputMaps;

namespace Super_Duper_Shooter.Classes.GameObjects;

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
    private SpriteBatch _spriteBatch;
    private ContentManager _contentManager; //to load animation textures
    public PlayerState CurrentState;

    private AnimationPlayer _animator;

    //The sprite that will be shown by default if no animation is yet set
    private const string DefaultSprite = "Placeholder Texture";

    private const string IdleSprite = "Characters/Jet Boxer (Idle)";
    private const string WalkingSprite = "Characters/Denji-Walking";
    private const string ShootingSprite = "Characters/Denji - Knockup Attack (Correct)";
    private const string AttackingSprite = "Characters/Denji - Knockup Attack";
    private Animation _idle;
    private Animation _walking;
    private Animation _shooting;

    private const float MoveSpeed = 500;
    private readonly InputManager _inputManager;

    public Texture2D Texture => _texture;

    private Vector2 _moveInput;// (horizontal, vertical)

    private List<Projectile> Projectiles;
    private bool hasFiredLeft;
    private bool hasFireRight;

    public Player(Vector2 playerPos, SpriteBatch spriteBatch, ContentManager contentManager, InputManager inputManager)
    {
        Position = playerPos;
        _spriteBatch = spriteBatch;
        _contentManager = contentManager;
        _inputManager = inputManager;
        
        //The texture will define the actual size of the player sprite
        _texture = contentManager.Load<Texture2D>(IdleSprite); //default texture
        _moveInput = Vector2.Zero;
        _animator = new AnimationPlayer(_spriteBatch, this);
        Projectiles = new List<Projectile>();
        
        SetAnimations();
        SwitchState(PlayerState.Idle); //default state
    }
    
    private void SetAnimations()//based on the resolution, accordingly sized sprites will be rendered
    {
        _idle = new Animation(_contentManager.Load<Texture2D>(IdleSprite), 80, 96, 0.1f, true);
        _walking = new Animation(_contentManager.Load<Texture2D>(WalkingSprite), 0.1f, true);
        _shooting = new Animation(_contentManager.Load<Texture2D>(AttackingSprite), 96, 80, 0.1f, false);
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
        
        _position = new Vector2(_position.X, _position.Y - MoveSpeed * Time.DeltaTime * _moveInput.Y);
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

        foreach (var projectile in Projectiles)
        {
            projectile.MoveUp();
            
            //If projectile is outside the boundaries of the screen, remove it from list
            if (projectile.Position.Y > spriteBatch.GraphicsDevice.Viewport.Height)
            {
                Projectiles.Remove(projectile);
                Console.WriteLine("Projectile was deleted");
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
        _animator.Play(_walking);
        _animator.IsFlipped = true;
        _position = new Vector2(_position.X - MoveSpeed * Time.DeltaTime, _position.Y);

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
        _animator.Play(_walking);
        _animator.IsFlipped = false;
        _position = new Vector2(_position.X + MoveSpeed * Time.DeltaTime, _position.Y);

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
        
        if(!hasFiredLeft)
        {
            Projectiles.Add(new Projectile(
            new Vector2(Position.X - 25, Position.Y), 
            _spriteBatch,
            _contentManager, 2));

            hasFiredLeft = true;
        }

        if (_animator.NormalizedTime >= 1.0f) //if the shooting animation ended, return to normal
        {
            SwitchState(PlayerState.Idle);
            hasFiredLeft = false;
        }
    }
    
    private void ShootRight()
    {
        _animator.Play(_shooting);

        if (!hasFireRight)
        {
            Projectiles.Add(new Projectile(
                new Vector2(Position.X + 25, Position.Y),
                _spriteBatch,
                _contentManager, 2));

            hasFireRight = true;
        }

        if (_animator.NormalizedTime >= 1.0f) //if the shooting animation ended, return to normal
        {
            SwitchState(PlayerState.Idle);
            hasFireRight = false;
        }
    }
}