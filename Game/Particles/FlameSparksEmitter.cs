using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Duper_Shooter.Engine.Particles;
using Super_Duper_Shooter.Engine.Particles.EmitterTypes;

namespace Super_Duper_Shooter.Game.Particles;

public class FlameSparksEmitter : Emitter
{
    public const string SpriteName = "Effects/Spark";
    
    private const int NbParticles = 10;
    private const int MaxParticles = 1000;
    private static Vector2 Direction = new Vector2(0.0f, 1.0f);
    private const float Spread = 1.5f;
    
    public FlameSparksEmitter(Texture2D texture, Vector2 position) : base(texture, position, new FlameSparkParticleState(), new ConeEmitter(Direction, Spread), NbParticles, MaxParticles)
    {
    }
}