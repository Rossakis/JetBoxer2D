using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.GameObjects.Base;
using Super_Duper_Shooter.Classes.InputSystem;
using Super_Duper_Shooter.Classes.InputSystem.InputMaps;
using Super_Duper_Shooter.ContentGeneration;

namespace Super_Duper_Shooter.Classes.GameObjects;

public enum PlayerState
{
    Idle,
    MovingLeft,
    MovingRight,
    Shooting,
    Death
}

public class Player : BaseGameObject
{
    public Vector2 PlayerPos { get; set; }

    private SpriteBatch _spriteBatch;
    private ContentManager _contentManager; //to load animation textures
    public PlayerState CurrentState;

    private AnimationPlayer _animator;

    //The sprite that will be shown by default if no animation is yet set
    private const string DefaultSprite = "Placeholder Texture";

    private const string IdleSprite = "Characters/Denji-Idle (Hand Drawn) Small";
    private const string WalkingSprite = "Characters/Denji-Walking";
    private const string ShootingSprite = "Characters/Denji - Knockup Attack (Correct)";
    private const string AttackingSprite = "Characters/Denji - Knockup Attack";
    private Animation _idle;
    private Animation _walking;
    private Animation _shooting;

    private const float MoveSpeed = 500;
    private InputManager _inputManager;

    public Texture2D Texture => _texture;

    public Player(Vector2 playerPos, SpriteBatch spriteBatch, ContentManager contentManager, InputManager inputManager)
    {
        _position = playerPos;
        _spriteBatch = spriteBatch;
        _contentManager = contentManager;
        _inputManager = inputManager;
        
        _texture = contentManager.Load<Texture2D>(DefaultSprite); //default texture
        InitializeAnimator();
        SwitchState(PlayerState.Idle); //default state
    }

    private void InitializeAnimator()
    {
        _animator = new AnimationPlayer(_spriteBatch, this);

        _idle = new Animation(_contentManager.Load<Texture2D>(IdleSprite), 0.1f, true);
        _walking = new Animation(_contentManager.Load<Texture2D>(WalkingSprite), 0.1f, true);
        _shooting = new Animation(_contentManager.Load<Texture2D>(AttackingSprite), 96, 80, 0.1f, false);
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
            case PlayerState.Shooting:
                ShootProjectile();
                break;
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
        
        if (_inputManager.GetAxis(GameplayInputMap.MoveHorizontal.InputAction).X < 0)
            SwitchState(PlayerState.MovingLeft);
        if (_inputManager.GetAxis(GameplayInputMap.MoveHorizontal.InputAction).X > 0)
            SwitchState(PlayerState.MovingRight);
        
        if (_inputManager.GetButtonDown(GameplayInputMap.ShootLeft.InputAction) || _inputManager.GetButtonDown(GameplayInputMap.ShootRight.InputAction))
            SwitchState(PlayerState.Shooting);
    }

    private void MoveLeft()
    {
        _animator.Play(_walking);
        _animator.IsFlipped = true;
        PlayerPos = new Vector2(PlayerPos.X - MoveSpeed * Time.DeltaTime, PlayerPos.Y);

        if (_inputManager.GetAxis(GameplayInputMap.MoveHorizontal.InputAction).X == 0)
            SwitchState(PlayerState.Idle);
        
        if (_inputManager.GetAxis(GameplayInputMap.MoveHorizontal.InputAction).X > 0)
            SwitchState(PlayerState.MovingRight);
        
        if (_inputManager.GetButtonDown(GameplayInputMap.ShootLeft.InputAction) || _inputManager.GetButtonDown(GameplayInputMap.ShootRight.InputAction))
            SwitchState(PlayerState.Shooting);
    }

    private void MoveRight()
    {
        _animator.Play(_walking);
        _animator.IsFlipped = false;
        PlayerPos = new Vector2(PlayerPos.X + MoveSpeed * Time.DeltaTime, PlayerPos.Y);

        if (_inputManager.GetAxis(GameplayInputMap.MoveHorizontal.InputAction).X == 0)
            SwitchState(PlayerState.Idle);
        
        if (_inputManager.GetAxis(GameplayInputMap.MoveHorizontal.InputAction).X < 0)
            SwitchState(PlayerState.MovingLeft);
        
        if (_inputManager.GetButtonDown(GameplayInputMap.ShootLeft.InputAction) || _inputManager.GetButtonDown(GameplayInputMap.ShootRight.InputAction))
            SwitchState(PlayerState.Shooting);    }

    private void ShootProjectile()
    {
        _animator.Play(_shooting);

        if (_animator.NormalizedTime >= 1.0f) //if the shooting animation ended, return to normal
            SwitchState(PlayerState.Idle);
    }
}