using ImprovedPerlinNoiseProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class MeshTopologyWorker : MonoBehaviour
{
    public ComputeShader m_perlinNoise;
    private ComputeBuffer m_noiseBuffer;
    private static int N = 8;
    public Vector3 chunkId = new Vector3(0, 0, 0);

    public void Init()
    {
        m_perlinNoise = UnityEngine.Object.Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise2D"));
        m_noiseBuffer = new ComputeBuffer((LowPolyTerrain2D.instance.chunk_size + N) * (LowPolyTerrain2D.instance.chunk_size + N) * 6, sizeof(float) * 3);
        m_perlinNoise.SetInt("_Width", LowPolyTerrain2D.instance.chunk_size + N);
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
        chunkId = offset;
        m_perlinNoise.SetFloat("_X", offset.x * LowPolyTerrain2D.instance.chunk_size);
        m_perlinNoise.SetFloat("_Y", offset.y * LowPolyTerrain2D.instance.chunk_size);
        m_perlinNoise.SetFloat("_Z", offset.z * LowPolyTerrain2D.instance.chunk_size);
        m_perlinNoise.Dispatch(PerlinAPI.p2d.type, (LowPolyTerrain2D.instance.chunk_size + N) / N, (LowPolyTerrain2D.instance.chunk_size + N) / N, 1);
        AsyncGPUReadback.Request(m_noiseBuffer, GetNoiseDataCallback);
        //AsyncGPUReadback.WaitAllRequests();
    }

    public void GetNoiseDataCallback(AsyncGPUReadbackRequest request)
    {
        Vector3[] data = request.GetData<Vector3>().ToArray();
    }

    public class Pool : MonoMemoryPool<MeshTopologyWorker>
    {
        protected override void OnCreated(MeshTopologyWorker worker)
        {
            worker.Init();
        }
        protected override void OnDestroyed(MeshTopologyWorker worker)
        {
            worker.m_noiseBuffer.Release(); ;
        }
        protected override void Reinitialize(MeshTopologyWorker worker)
        {

        }
    }

}
