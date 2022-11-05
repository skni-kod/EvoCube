using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class TerrainResourceManager : MonoBehaviour
{
    public class TopologyWorker
    {
        public ComputeShader m_perlinNoise;
        private ComputeBuffer m_noiseBuffer;
        //public Vector3 chunkId = new Vector3(0, 0, 0);

        public void Init()
        {
            m_perlinNoise = Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise2D"));
            m_noiseBuffer = new ComputeBuffer((TerrainConfig.chunkSize + TerrainConfig.kernelNumber) * (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) * 6, sizeof(float) * 3);
            m_perlinNoise.SetInt("_Width", TerrainConfig.chunkSize + TerrainConfig.kernelNumber);
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

        public void Generate(Vector3 offset, Action<AsyncGPUReadbackRequest> callback)
        {
            m_perlinNoise.SetFloat("_X", offset.x * TerrainConfig.chunkSize);
            m_perlinNoise.SetFloat("_Y", offset.y * TerrainConfig.chunkSize);
            m_perlinNoise.SetFloat("_Z", offset.z * TerrainConfig.chunkSize);
            m_perlinNoise.Dispatch(PerlinAPI.p2d.type, (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) / TerrainConfig.kernelNumber, (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) / TerrainConfig.kernelNumber, 1);
            AsyncGPUReadback.Request(m_noiseBuffer, callback);
        }

        public class Pool : MemoryPool<TopologyWorker>
        {
        }
    }

}
