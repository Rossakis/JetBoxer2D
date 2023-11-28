using System;
using System.Collections.Generic;
using JetBoxer2D.Engine.Objects;
using JetBoxer2D.Engine.Particles.EmitterTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JetBoxer2D.Engine.Particles;

/// <summary>
/// Class responsible for emitting particles
/// </summary>
public class Emitter : BaseGameObject
{
    private LinkedList<Particle> _activeParticles = new();
    private LinkedList<Particle> _inactiveParticles = new();

    public EmitterParticleState _emitterParticleState;
    private IEmitterType _emitterType;
    private int _nbParticleEmittedPerUpdate = 0;
    private int _maxNbParticle = 0;

    public bool IsAlive => _activeParticles.Count > 0;
    private bool _active = true;
    public int Age { get; set; }
    public float Rotation { get; set; }

    public Emitter(Texture2D texture, Vector2 position, float rotation, EmitterParticleState particleState,
        IEmitterType emitterType,
        int nbParticleEmittedPerUpdate, int maxNbParticle)
    {
        _texture = texture;
        Position = position;
        Rotation = rotation;
        _emitterParticleState = particleState;
        _emitterType = emitterType;
        _nbParticleEmittedPerUpdate = nbParticleEmittedPerUpdate;
        _maxNbParticle = maxNbParticle;
        Rotation =
            Age = 0;
    }

    private void EmitNewParticle(Particle particle)
    {
        var lifespan = _emitterParticleState.GenerateLifespan();
        var velocity = _emitterParticleState.GenerateVelocity();
        var scale = _emitterParticleState.GenerateScale();
        var rotation = _emitterParticleState.GenerateRotation();
        var opacity = _emitterParticleState.GenerateOpacity();
        var gravity = _emitterParticleState.Gravity;
        var acceleration = _emitterParticleState.Acceleration;
        var opacityFadingRate = _emitterParticleState.OpacityFadingRate;

        var direction = _emitterType.GetParticleDirection();
        var position = _emitterType.GetParticlePosition(_position);

        particle.Activate(lifespan, position, direction, gravity, velocity, acceleration, scale, rotation, opacity,
            opacityFadingRate);
        _activeParticles.AddLast(particle);
    }

    private void EmitParticles()
    {
        // make sure we're not at max particles
        if (_activeParticles.Count >= _maxNbParticle)
            return;

        var maxAmountToBeGenerated = _maxNbParticle - _activeParticles.Count;
        var neededParticles = Math.Min(maxAmountToBeGenerated, _nbParticleEmittedPerUpdate);

        //Reuse inactive particles first before creating new ones
        var nbToReuse = Math.Min(_inactiveParticles.Count, neededParticles);
        var nbToCreate = neededParticles - nbToReuse;

        for (var i = 0; i < nbToReuse; i++)
        {
            var particleNode = _inactiveParticles.First;
            EmitNewParticle(particleNode.Value);
            _inactiveParticles.Remove(particleNode);
        }

        for (var i = 0; i < nbToCreate; i++) EmitNewParticle(new Particle());
    }

    public override void Update()
    {
        if (_active) EmitParticles();

        var particleNode = _activeParticles.First;
        while (particleNode != null)
        {
            var nextNode = particleNode.Next;
            var stillAlive = particleNode.Value.Update();

            if (!stillAlive)
            {
                _activeParticles.Remove(particleNode);
                _inactiveParticles.AddLast(particleNode.Value);
            }

            particleNode = nextNode;
        }

        Age++;
    }

    public void Deactivate()
    {
        _active = false;
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
        var rotationAngle = (float) (Math.PI / 2f - Rotation);

        foreach (var particle in _activeParticles)
            //TODO: implement the rotation parameter once rotation function gets added to EmitterParticleState
            spriteBatch.Draw(_texture, particle.Position, sourceRectangle, Color.White * particle.Opacity,
                rotationAngle, new Vector2(0, 0), particle.Scale, SpriteEffects.None, zIndex);
    }
}