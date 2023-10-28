using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.GameObjects.Base;

namespace Super_Duper_Shooter.Classes.Extensions.ContentGeneration;

/// <summary>
/// This class is created in order to process png sprite sheets in the form of a single horizontal line
/// </summary>
public class Animation : BaseGameObject
{
    public Texture2D Texture => _texture;
    public float FrameTime { get; }
    public bool IsLooping { get; }

    public int AmountOfFrames
    {
        get
        {
            //In case the Width is smaller than the Height (in a case of a small & single-frame animation),
            //indicate that there is only one frame, and NOT zero frames, so that AnimationPlayer doesn't assume
            //AmountOfFrames = 0 in its calculations 
            if (_texture.Width / _texture.Height < 1)
                return 1;
            
            return _texture.Width / _texture.Height;
        }
        
    }

    //By default, if no parameters for the width/height of the animation are given,
    //then the width and height will be decided by the texture's height only, since a single sprite sheet
    //(texture) can contain the total width of ALL of the sprite's frames e.g. A sprite sheet of size 512x64
    public Animation(Texture2D texture, float frameTime, bool isLooping)
    {
        _texture = texture;
        FrameTime = frameTime;
        IsLooping = isLooping;

        Width = _texture.Height;
        Height = _texture.Height;
    }

    //Specify explicitly the frame size of each sprite in the animation
    public Animation(Texture2D texture, int frameWidth, int frameHeight, float frameTime, bool isLooping)
    {
        _texture = texture;
        FrameTime = frameTime;
        IsLooping = isLooping;

        Width = frameWidth;
        Height = frameHeight;
    }
}