using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Engine.Objects;

namespace Super_Duper_Shooter.Game.Objects;

public class SplashImage : BaseGameObject
{
    public SplashImage(Texture2D texture)
    {
        _texture = texture;
    }
}