using ImprovedPerlinNoiseProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PerlinAPI : MonoBehaviour
{
    public static ComputeShader m_perlinNoise;
    static ComputeBuffer m_noiseBuffer;
    static GPUPerlinNoise perlin;
    private static int N = 8;
    private static PerlinAPI instance = null;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_perlinNoise = Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise2D");
    }

    public static float[] GPUPerlin2D(int size, int seed, Vector2 offset, float gain, float frequency, float lacunarity, float idk, int type, int octaves)
    {
        int corrected_size = size;
        while (corrected_size % N != 0)
            corrected_size++;
        float[] map = GPUPerlin2dMAP(corrected_size, seed, offset, gain, frequency, lacunarity, idk, type, octaves);
        float[] new_map = new float[size * size];
        int idx = 0;
        for (int y = 0; y < corrected_size; y++)
        {
            for (int x = 0; x < corrected_size; x++)
            {
                if (x < size && y < size)
                {
                    new_map[idx++] = map[x + y * corrected_size];
                }
            }
        }
        return new_map;
    }

    private static float[] GPUPerlin2dMAP(int size, int seed, Vector2 offset, float gain, float frequency, float lacunarity, float idk, int type, int octaves)
    {
        if (type > 4 || type <0)
        {
            throw new System.ArgumentException("Ther is no such type");
        }
        if (size % N != 0)
        {
            //There are 9 threads run per group so size must be divisible by N.
            throw new System.ArgumentException("N must be divisible be {N}");
        }

        //Holds the values, generated from perlin noise.
        m_noiseBuffer = new ComputeBuffer(size * size, sizeof(float));

        perlin = new GPUPerlinNoise(seed);
        perlin.LoadResourcesFor2DNoise();

        m_perlinNoise.SetInt("_Width", size);
        m_perlinNoise.SetFloat("_Frequency", frequency);
        m_perlinNoise.SetFloat("_Lacunarity", lacunarity);
        m_perlinNoise.SetFloat("_X", offset.x);
        m_perlinNoise.SetFloat("_Y", offset.y);
        m_perlinNoise.SetInt("_Octaves", octaves);
        m_perlinNoise.SetFloat("_Idk", idk);
        m_perlinNoise.SetFloat("_Gain", gain);
        m_perlinNoise.SetTexture(type, "_PermTable1D", perlin.PermutationTable1D);
        m_perlinNoise.SetTexture(type, "_Gradient2D", perlin.Gradient2D);
        m_perlinNoise.SetBuffer(type, "_Result", m_noiseBuffer);

        m_perlinNoise.Dispatch(type, size / N, size / N, 1);
        //m_perlinNoise.FindKernel("ridge");

        //Get the data out of the buffer.
        float[] verts = new float[size * size];
        m_noiseBuffer.GetData(verts);
        m_noiseBuffer.Release();
        return verts;
    }
}
