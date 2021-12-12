using ImprovedPerlinNoiseProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PerlinAPI : MonoBehaviour
{
    public static ComputeShader m_perlinNoise;
    public static int seed = 0;
    public static GPUPerlinNoise perlin;
    public static Perlin2dSettings p2d;
    [SerializeField] private Perlin2dSettings p2d_to_set;
    public static int N = 8;
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
        ReloadPerlin();
    }
    public void ReloadPerlin()
    {
        p2d = p2d_to_set;
        m_perlinNoise = Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise2D");
        perlin = new GPUPerlinNoise(seed);
        perlin.LoadResourcesFor2DNoise();
    }

    #region Main Methods

    public static float[] GPUPerlin2D(int size, Vector3 offset)
    {
        int corrected_size = size;
        while (corrected_size % N != 0)
            corrected_size++;
        //float[] map = GPUPerlin2dMAP(offset, corrected_size);
        float[] new_map = new float[size * size];
        int idx = 0;
        for (int y = 0; y < corrected_size; y++)
        {
            for (int x = 0; x < corrected_size; x++)
            {
                if (x < size && y < size)
                {
                    //new_map[idx++] = map[x + y * corrected_size];
                }
            }
        }
        return new_map;
    }

    /*private static float[] GPUPerlin2dMAP(Vector3 offset, int size)
    {
        if (p2d.type > 4 || p2d.type <0)
        {
            throw new System.ArgumentException("Ther is no such type");
        }
        //if (chunk_size % N != 0)
        {
            //There are 9 threads run per group so size must be divisible by N.
            throw new System.ArgumentException("N must be divisible be {N}");
        }

        //Holds the values, generated from perlin noise.
        //ComputeBuffer m_noiseBuffer = new ComputeBuffer(size * size, sizeof(float));

        //perlin = new GPUPerlinNoise(seed);
        //perlin.LoadResourcesFor2DNoise();

        m_perlinNoise.SetInt("_Width", chunk_size);
        m_perlinNoise.SetFloat("_Frequency", p2d.frequency);
        m_perlinNoise.SetFloat("_Lacunarity", p2d.lacunarity);
        m_perlinNoise.SetFloat("_X", offset.x);
        m_perlinNoise.SetFloat("_Z", offset.z);
        m_perlinNoise.SetInt("_Octaves", p2d.octaves);
        m_perlinNoise.SetFloat("_Idk", p2d.idk);
        m_perlinNoise.SetFloat("_Gain", p2d.gain);
        m_perlinNoise.SetTexture(p2d.type, "_PermTable1D", perlin.PermutationTable1D);
        m_perlinNoise.SetTexture(p2d.type, "_Gradient2D", perlin.Gradient2D);
        //m_perlinNoise.SetBuffer(p2d.type, "_Result", m_noiseBuffer);

        m_perlinNoise.Dispatch(p2d.type, size / N, size / N, 1);
        //m_perlinNoise.FindKernel("ridge");

        //Get the data out of the buffer.
        float[] verts = new float[size * size];
        //m_noiseBuffer.GetData(verts);
        //m_noiseBuffer.Release();
        return verts;
    }*/
    #endregion

}
