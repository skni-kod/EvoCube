using ImprovedPerlinNoiseProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PerlinGenerator : IObjectPool
{
    public static ComputeShader m_perlinNoise;
    private ComputeBuffer m_noiseBuffer;
    private static int N = 8;
    private float[] verts = new float[PerlinAPI.chunk_size * PerlinAPI.chunk_size];


    #region Main Methods
    public PerlinGenerator()
    {

    }
    
    public void OnRelease()
    {
        m_noiseBuffer.Release();
    }

    public void Init()
    {
        m_perlinNoise = Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise2D");
        m_noiseBuffer = new ComputeBuffer(PerlinAPI.chunk_size * PerlinAPI.chunk_size, sizeof(float));
        m_perlinNoise.SetInt("_Width", PerlinAPI.chunk_size);
        m_perlinNoise.SetTexture(PerlinAPI.p2d.type, "_PermTable1D", PerlinAPI.perlin.PermutationTable1D);
        m_perlinNoise.SetTexture(PerlinAPI.p2d.type, "_Gradient2D", PerlinAPI.perlin.Gradient2D);
        m_perlinNoise.SetBuffer(PerlinAPI.p2d.type, "_Result", m_noiseBuffer);
        ReloadSettings();
    }

    void ReloadSettings()
    {
        m_perlinNoise.SetInt("_Octaves", PerlinAPI.p2d.octaves);
        m_perlinNoise.SetFloat("_Idk", PerlinAPI.p2d.idk);
        m_perlinNoise.SetFloat("_Gain", PerlinAPI.p2d.gain);
        m_perlinNoise.SetFloat("_Frequency", PerlinAPI.p2d.frequency);
        m_perlinNoise.SetFloat("_Lacunarity", PerlinAPI.p2d.lacunarity);
    }

    public void Generate(Vector3 offset)
    {
        m_perlinNoise.SetFloat("_X", offset.x);
        m_perlinNoise.SetFloat("_Z", offset.z);
        m_perlinNoise.Dispatch(PerlinAPI.p2d.type, PerlinAPI.chunk_size / N, PerlinAPI.chunk_size / N, 1);
        AsyncGPUReadback.Request(m_noiseBuffer, GetNoiseDataCallback);
    }

    public void GetNoiseDataCallback(AsyncGPUReadbackRequest request)
    {
        request.WaitForCompletion();
        verts = request.GetData<float>().ToArray();
        LowPolyTerrain.instance.GenerateChunk(verts);
    }

    #endregion


    #region Thread Methods

    #endregion



}
