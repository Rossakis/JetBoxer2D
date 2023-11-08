using System;
using JetBoxer2D.Engine.Extensions;
using JetBoxer2D.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Engine.Animation;

public class AnimationPlayer : BaseGameObject
{
    private readonly SpriteBatch _spriteBatch; //render the animation with this
    private readonly BaseGameObject _animatedGameObject; //the animated game object's position and other data, which the animation needs

    private int _currentFrame;
    private float _timer; //Timer to calculate the duration of _currentFrame
    
    public float AnimationSpeed { get; set; }
    public AnimationClip Animation { get; private set; }
    public float NormalizedTime { get; private set; } //E.g. 0.5f would be considered the animation at its half time
    public bool IsFlipped { get; set; }
    public bool IsStopped { get; set; }
    public bool HasEnded => NormalizedTime >= 1f;
    public float Rotation { get; set; }


    public AnimationPlayer(SpriteBatch spriteBatch, BaseGameObject animatedGameObj)
    {
        _spriteBatch = spriteBatch;
        _animatedGameObject = animatedGameObj;
        _timer = 0.0f;
        _currentFrame = 0;
        AnimationSpeed = 1f;
    }

    public void Play(AnimationClip animation)
    {
        if(IsStopped)
            return;
        
        //Error handling
        if(animation == null) throw new Exception($"Animation instance wasn't defined");

        //If new animation is set
        if (animation != Animation)
        {
            _currentFrame = 0;
            _timer = 0f;
            _texture = animation.Texture;
            Animation = animation;
        }
        _timer += Time.DeltaTime;
        CalculateNormalizedTime(animation);
        
        while (_timer > animation.FrameTime)
        {
            //reset timer
            _timer -= animation.FrameTime /
                      AnimationSpeed; //the bigger the animationSpeed value, the faster the animation

            if (animation.IsLooping) //when looping, reset currentFrame to zero
                _currentFrame = (_currentFrame + 1) % animation.AmountOfFrames;
            else //leave current frame as is
                _currentFrame = Math.Min(_currentFrame + 1, animation.AmountOfFrames);
        }
        
        //Animation frame target
        var sourceRectangle = new Rectangle(_currentFrame * animation.Width, 0,
            animation.Width,
            animation.Height);

        //Animation projection target
       var destinationRectangle = new Rectangle((int) _animatedGameObject.Position.X, (int) _animatedGameObject.Position.Y,
            animation.Width,
            animation.Height);
        
        if (IsFlipped)
        {
            _spriteBatch.Draw(animation.Texture, destinationRectangle, sourceRectangle, Color.White, 
                Rotation, animation.Centre, SpriteEffects.FlipHorizontally, 1);
        }
        else
        {
            _spriteBatch.Draw(animation.Texture, destinationRectangle, sourceRectangle, Color.White, 
                Rotation, animation.Centre, SpriteEffects.None, 1);
        }
    }

    //Calculate the normalizedTime of the animation
    private void CalculateNormalizedTime(AnimationClip animation)
    {
        NormalizedTime = (_currentFrame + 1) / (float) animation.AmountOfFrames;
    }
}