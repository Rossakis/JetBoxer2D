using JetBoxer2D.Engine.Particles;
using Microsoft.Xna.Framework;

namespace JetBoxer2D.Game.Particles;

public class FlameSparkParticleState : EmitterParticleState
{
    public override int MinLifeSpan => 60;
    public override int MaxLifeSpan => 90;
    public override float Velocity => 4.0f;
    public override float VelocityDeviation => 1.0f;
    public override float Acceleration => 0.8f;
    public override Vector2 Gravity => new Vector2(0, 0);
    public override float Opacity => 0.4f;
    public override float OpacityDeviation => 0.1f;
    public override float OpacityFadingRate => 0.86f;
    public override float Rotation => 0.0f;
    public override float RotationDeviation => 0.0f;
    public override float Scale => 0.75f;
    public override float ScaleDeviation => 0.05f;
}