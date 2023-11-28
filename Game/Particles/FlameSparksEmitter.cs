using JetBoxer2D.Engine.Particles;
using JetBoxer2D.Engine.Particles.EmitterTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Game.Particles;

public class FlameSparksEmitter : Emitter
{
    public const string SpriteName = "Effects/Spark";

    private const int NbParticles = 10;
    private const int MaxParticles = 1000;
    private static Vector2 Direction = new(0.0f, 1.0f);
    private const float Spread = 1.5f;

    public FlameSparksEmitter(Texture2D texture, Vector2 position, float rotation) : base(texture, position, rotation,
        new FlameSparkParticleState(), new ConeEmitter(Direction, Spread), NbParticles, MaxParticles)
    {
    }
}