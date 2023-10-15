using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Enums;

namespace Super_Duper_Shooter.Classes.GameObjects.Base;

public class BaseGameObject
{
    protected Texture2D _texture;
    protected Vector2 _position;
    public int Width;
    public int Height;

    private Rectangle _destinationRectangle;
    private Rectangle _sourceRectangle;


    public int zIndex;

    public virtual void OnNotify(Events eventType)
    {
    }

    /// <summary>
    /// Render this gameObject's sprites
    /// </summary>
    /// <param name="spriteBatch"></param>
    public virtual void Render(SpriteBatch spriteBatch)
    {
        // TODO: Drawing call here
        if (_texture != null)
            spriteBatch.Draw(_texture, _position, Color.White);
        else
            throw new Exception($"{ToString()} texture field wasn't defined");
    }

    /// <summary>
    /// Get the texture's center point, used when calling the Draw() method
    /// </summary>
    public Vector2 CenterTexture()
    {
        return new Vector2(Width / 2f, Height / 2f);
    }
}