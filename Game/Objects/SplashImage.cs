using JetBoxer2D.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Game.Objects;

public class SplashImage : BaseGameObject
{
    public SplashImage(Texture2D texture)
    {
        _texture = texture;
    }
}