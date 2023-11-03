using Microsoft.Xna.Framework;

namespace Super_Duper_Shooter.Engine.Particles;

/// <summary>
/// A class used by the ParticleEmitterState to draw particle effects on the screen
/// </summary>
public class Particle
{
    //Since Monogame's Update() method gets called 60 times per second, _lifespan should be equal 
    //to the lifespan seconds * 60. Aka, lifespan = 180 means that a particle will exist for 3 seconds
    private int _lifespan;
    private int _age;//current age in number of frames
    private Vector2 _gravity;
    private Vector2 _direction;
    private float _velocity;
    private float _acceleration;
    private float _rotation;//TODO: add continuous rotation function to the particle
    private float _opacityFadingRate;
    
    public Vector2 Position { get; set; }
    public float Scale { get; set; }
    public float Opacity { get; set; }//from 0 to 1

    public Particle()
    { }

    public void Activate(int lifespan, Vector2 position, Vector2 direction, Vector2 gravity, float velocity, float acceleration, float scale, float rotation, float opacity, float opacityFadingRate)
    {
        _lifespan = lifespan;
        _gravity = gravity;
        _direction = direction;
        _velocity = velocity;
        _acceleration = acceleration;
        _rotation = rotation;
        _opacityFadingRate = opacityFadingRate;

        Position = position;
        Scale = scale;
        Opacity = opacity;
    }

    public bool Update()
    {
        _velocity *= _acceleration;
        _direction += _gravity;//particle slowly turns towards the gravity force

        var positionDelta = _direction * _velocity;
        Position += positionDelta;
        Opacity *= _opacityFadingRate;
        
        _age++;
        
        return _age < _lifespan;//returns false when particle dies
    }

}