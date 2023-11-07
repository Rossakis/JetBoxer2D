using System;

namespace JetBoxer2D.Engine.Extensions;

/// <summary>
/// A helper class, to help us use Random generator with float numbers more easily
/// </summary>
public class RandomNumberGenerator
{
    private Random _random = new();

    public int NextRandom() => _random.Next();
    public int NextRandom(int max) => _random.Next(max); 
    public int NextRandom(int min, int max) => _random.Next(min, max);


    public float NextRandom(float max) => (float) _random.NextDouble() * max;
    public float NextRandom(float min, float max) => (float) _random.NextDouble() * (max - min) + min;
}