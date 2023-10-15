using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.Extensions;
using Super_Duper_Shooter.Classes.GameObjects;
using Super_Duper_Shooter.Classes.GameObjects.Base;

namespace Super_Duper_Shooter.ContentGeneration;

public class AnimationPlayer : BaseGameObject
{
    private readonly SpriteBatch _spriteBatch; //render the animation with this
    private readonly Player _player; //player position and other data, which animation needs to correctly render

    private int _currentFrame;
    private float _timer; //Timer to calculate the duration of _currentFrame

    public float AnimationSpeed { get; set; }
    public Animation Animation { get; private set; }
    public float NormalizedTime { get; private set; } //E.g. 0.5f would be considered the animation at its half time
    public bool IsFlipped { get; set; }


    public AnimationPlayer(SpriteBatch spriteBatch, Player player)
    {
        _spriteBatch = spriteBatch;
        _player = player;
        _timer = 0.0f;
        _currentFrame = 0;
        AnimationSpeed = 1f;
    }

    public void Play(Animation animation)
    {
        //Error handling
        Animation = animation ?? throw new Exception($"{ToString()} Animation instance wasn't defined");

        _texture = animation.Texture;
        _timer += Time.DeltaTime;
        CalculateNormalizedTime(animation);

        while (_timer > animation.FrameTime)
        {
            //reset timer
            _timer -= animation.FrameTime /
                      AnimationSpeed; //the bigger the animationSpeed value, the faster the animation framerate

            if (animation.IsLooping) //when looping, reset currentFrame to zero
                _currentFrame = (_currentFrame + 1) % animation.AmountOfFrames;
            else //leave current frame as is
                _currentFrame = Math.Min(_currentFrame + 1, animation.AmountOfFrames);
        }

        var sourceRectangle = new Rectangle(_currentFrame * animation.Width, 0,
            animation.Width,
            animation.Height); //animation.Width and animation.Height are the same in this instance

        if (IsFlipped)
        {
            var destinationRectangle = new Rectangle((int) _player.PlayerPos.X, (int) _player.PlayerPos.Y,
                animation.Width,
                animation.Height);
            _spriteBatch.Draw(animation.Texture, destinationRectangle, sourceRectangle, Color.White, 0, Vector2.Zero,
                SpriteEffects.FlipHorizontally, 0);
        }
        else
        {
            var destinationRectangle = new Rectangle((int) _player.PlayerPos.X, (int) _player.PlayerPos.Y,
                animation.Height,
                animation.Height);
            _spriteBatch.Draw(animation.Texture, destinationRectangle, sourceRectangle, Color.White, 0,
                animation.CenterTexture(),
                SpriteEffects.None, 1);
        }
    }

    //Calculate the normalizedTime of the animation
    private void CalculateNormalizedTime(Animation animation)
    {
        NormalizedTime = (_currentFrame + 1) / (float) animation.AmountOfFrames;
    }
}