using System;
using Microsoft.Xna.Framework;
using Super_Duper_Shooter.Engine.Extensions;

namespace Super_Duper_Shooter.Engine.Particles.EmitterTypes;

public class ConeEmitter : IEmitterType
{
    public Vector2 Direction { get; private set; }
    public float Spread { get; }

    private RandomNumberGenerator _random = new ();

    public ConeEmitter(Vector2 direction, float spread)
    {
        Direction = direction;
        Spread = spread;
    }
    
    public Vector2 GetParticleDirection()
    {
        if (Direction == null)
            return Vector2.Zero;

        var angle = (float) Math.Atan2(Direction.Y, Direction.X);// Math.Atan2() expects the y component first and x component second
        var newAngle = _random.NextRandom(angle - Spread / 2.0f, angle + Spread / 2.0f);
        
        var particleDirection = new Vector2((float) Math.Cos(newAngle), (float) Math.Sin(newAngle));
        particleDirection.Normalize();
        return particleDirection;
    }

    public Vector2 GetParticlePosition(Vector2 emitterPosition)
    {
        // return the same position for this type of emitter, but otherwise we could tweak this to start particles a bit further
        // away from the center of the cone.
        var x = emitterPosition.X;
        var y = emitterPosition.Y;

        return new Vector2(x, y);
    }
}