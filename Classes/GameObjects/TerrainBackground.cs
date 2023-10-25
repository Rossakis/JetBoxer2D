using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.GameObjects.Base;

namespace Super_Duper_Shooter.Classes.GameObjects;

public class TerrainBackground : BaseGameObject
{
    private const float ScrollingSpeed = 2.0f;
    private Player player;
    private SpriteBatch _spriteBatch;

    public TerrainBackground(Texture2D texture)
    {
        _texture = texture;
        Position = new Vector2(0, 0);
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        if (_texture == null)
            throw new Exception($"{ToString()} texture field wasn't defined");

        var viewport = _spriteBatch.GraphicsDevice.Viewport;
        var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);

        // (viewport.Height / _texture.Height + 1)  will give us the amount of textures we can fit horizontally on the screen
        //plus one more, which exists so that there doesn't appear a gap when the tiles move downwards
        for (var numVert = -1;
             numVert < viewport.Height / _texture.Height + 1;
             numVert++) //numVert = number of vertical tiles
        {
            var y = (int) Position.Y + numVert * _texture.Height;

            for (var numHor = -1; numHor < viewport.Width / _texture.Width + 1; numHor++)
            {
                var x = (int) Position.X + numHor * _texture.Width;
                var destinationRectangle =
                    new Rectangle(x, y, _texture.Width, _texture.Height);
                _spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White, 0,
                    CenterTexture(), SpriteEffects.None, zIndex);
            }
        }

        //Scrolling
        _position.Y = (int) (Position.Y + ScrollingSpeed) % _texture.Height;
    }
}