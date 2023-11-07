using System;
using Microsoft.Xna.Framework;

namespace JetBoxer2D.Engine.Physics;

public class Collider
{
    private Rectangle _rectangle;

    public event EventHandler<object> OnCollided;
    
    private void Collided(Collider collider)
    {
        OnCollided?.Invoke(this, EventArgs.Empty);
    }
}