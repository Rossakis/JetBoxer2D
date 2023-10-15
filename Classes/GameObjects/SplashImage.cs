using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Classes.GameObjects.Base;

namespace Super_Duper_Shooter.Classes.GameObjects;

public class SplashImage : BaseGameObject
{
    public SplashImage(Texture2D texture)
    {
        _texture = texture;
    }
}