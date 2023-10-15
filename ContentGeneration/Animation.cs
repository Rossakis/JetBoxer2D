using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.GameObjects.Base;

namespace Super_Duper_Shooter.ContentGeneration;

/// <summary>
/// This class is created in order to process png sprite sheets of only a single horizontal line of a character
/// </summary>
public class Animation : BaseGameObject
{
    //Width will be the same as the (texture's) Height because the sprite sheet can be e.g. 128x64 pixels
    //which means that to represent a single frame of the sprite sheet, both width and height need to be the same
    public int Width { get; }

    public int Height { get; }

    public Texture2D Texture { get; }

    public float FrameTime { get; }

    public bool IsLooping { get; }
    public int AmountOfFrames => Texture.Width / Texture.Height;

    //By default, if no parameters for the width/height of the animation are given,
    //then the width and height will be decided by the texture's height only, since a single sprite sheet
    //(texture) can contain the total width of ALL of the sprite's frames e.g. A sprite sheet of size 512x64
    public Animation(Texture2D texture, float frameTime, bool isLooping)
    {
        Texture = texture;
        FrameTime = frameTime;
        IsLooping = isLooping;

        Width = Texture.Height;
        Height = Texture.Height;
    }

    //Specify explicitly the frame size of each sprite in the animation
    public Animation(Texture2D texture, int frameWidth, int frameHeight, float frameTime, bool isLooping)
    {
        Texture = texture;
        FrameTime = frameTime;
        IsLooping = isLooping;

        Width = frameWidth;
        Height = frameHeight;
    }
}