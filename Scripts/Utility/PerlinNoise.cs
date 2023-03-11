using System.Drawing;
using System.Numerics;
using AutoBattleRPG.Scripts.Pathfinding;

namespace AutoBattleRPG.Scripts.Utility;

/// <summary>
///     Based on Perlin's original implementation from SIGGRAPH 2002 https://cs.nyu.edu/~perlin/noise/
///     And 3Dave's answer on this Stack Overflow post https://stackoverflow.com/a/8659483
/// </summary>
public static class PerlinNoise
{
    private const float Frequency = 0.5f;
    private const float Amplitude = 1f;
    
    private static readonly Vector2[] Gradients;
    private static int[] _permutation;

    static PerlinNoise()
    {
        GeneratePermutation(out _permutation);
        GenerateGradients(out Gradients);
    }

    private static void GeneratePermutation(out int[] permutation)
    {
        permutation = Enumerable.Range(0, 256).ToArray();
        
        for (var i = 0; i < permutation.Length; i++)
        {
            int source = RandomHelper.Rand.Next(permutation.Length);

            (permutation[i], permutation[source]) = (permutation[source], permutation[i]);
        }
    }
    
    public static void Reseed()
    {
        GeneratePermutation(out _permutation);
    }

    private static void GenerateGradients(out Vector2[] grad)
    {
        grad = new Vector2[256];

        for (var i = 0; i < grad.Length; i++)
        {
            Vector2 gradient;

            do
            {
                gradient = new Vector2((float)(RandomHelper.Rand.NextDouble() * 2 - 1), (float)(RandomHelper.Rand.NextDouble() * 2 - 1));
            }
            while (gradient.LengthSquared() >= 1);

            gradient = Vector2.Normalize(gradient);

            grad[i] = gradient;
        }
    }

    private static float Drop(float t)
    {
        t = Math.Abs(t);
        return 1f - t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Q(float u, float v)
    {
        return Drop(u) * Drop(v);
    }

    public static float Noise(float x, float y)
    {
        var cell = new Vector2((float)Math.Floor(x), (float)Math.Floor(y));

        var total = 0f;

        var corners = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

        foreach (var n in corners)
        {
            var ij = cell + n;
            var uv = new Vector2(x - ij.X, y - ij.Y);

            var index = _permutation[(int)ij.X % _permutation.Length];
            index = _permutation[(index + (int)ij.Y) % _permutation.Length];

            var grad = Gradients[index % Gradients.Length];

            total += Q(uv.X, uv.Y) * Vector2.Dot(grad, uv);
        }

        return Math.Max(Math.Min(total, 1f), -1f);
    }

    public static float[,] GenerateNoiseMap(int width, int height, int octaves)
    {
        float[,] texture = new float[width, height];
        
        float min = float.MaxValue;
        float max = float.MinValue;
        
        Reseed();

        float amplitude = Amplitude;
        float frequency = Frequency;

        for (var octave = 0; octave < octaves; octave++)
        {
            float freq = frequency;
            float ampl = amplitude;
            
            Parallel.For((long)0, width * height, (offset) =>
                {
                    long i = offset % width;
                    long j = offset / width;
                    var noise = Noise(i*freq*1f/width, j*freq*1f/height);
                    noise = texture[i,j] += noise * ampl;

                    min = Math.Min(min, noise);
                    max = Math.Max(max, noise);
                }
            );

            frequency *= 2;
            amplitude /= 2;
        }

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Normalization
                texture[x,y] = (texture[x,y] - min) / (max - min);
                Console.Write($"{texture[x,y] }");
            }
            Console.WriteLine();
        }

        return texture;
    }

    public static void TestMap()
    {
        GenerateNoiseMap(9, 9, 5);
    }
}