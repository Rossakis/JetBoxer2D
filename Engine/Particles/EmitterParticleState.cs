using JetBoxer2D.Engine.Extensions;
using Microsoft.Xna.Framework;

namespace JetBoxer2D.Engine.Particles;

public abstract class EmitterParticleState
{
    // how long a particle can live
    public abstract int MinLifeSpan { get; }
    public abstract int MaxLifeSpan { get; }
    
    // defines how the particles will move. Velocity and its deviation define the particle's initial velocity
    // then each update, we
    //  - increase velocity by acceleration
    //  - increase direction by gravity
    //  - multiply direction by velocity
    public abstract float Velocity { get; }
    public abstract float VelocityDeviation { get; }
    public abstract float Acceleration { get; }
    public abstract Vector2 Gravity { get; }
    
    public abstract float Opacity { get; }
    public abstract float OpacityDeviation { get; }
    public abstract float OpacityFadingRate { get; }
    
    public abstract float Rotation { get; }
    public abstract float RotationDeviation { get; }
    
    public abstract float Scale { get; }
    public abstract float ScaleDeviation { get; }

    private RandomNumberGenerator _random = new RandomNumberGenerator();

    public int GenerateLifespan()
    {
        return _random.NextRandom(MinLifeSpan, MaxLifeSpan);
    }

    public float GenerateVelocity()
    {
        return GenerateFloat(Velocity, VelocityDeviation);
    }

    public float GenerateOpacity()
    {
        return GenerateFloat(Opacity, OpacityDeviation);
    }

    public float GenerateRotation()
    {
        return GenerateFloat(Rotation, RotationDeviation);
    }

    public float GenerateScale()
    {
        return GenerateFloat(Scale, ScaleDeviation);
    }
    
    public float GenerateFloat(float startNum, float deviation)
    {
        var halfDeviation = deviation / 2f;
        return _random.NextRandom(startNum - halfDeviation, startNum + halfDeviation);
    }
}