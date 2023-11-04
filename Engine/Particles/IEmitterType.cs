using Microsoft.Xna.Framework;

namespace Super_Duper_Shooter.Engine.Particles;

public interface IEmitterType
{
    Vector2 GetParticleDirection();
    Vector2 GetParticlePosition(Vector2 emitterPosition);
}