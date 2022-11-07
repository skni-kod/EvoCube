using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace EvoCube.MapGeneration
{
    public class TopologyWorker : MonoBehaviour
    {
        public ComputeShader m_perlinNoise;
        private ComputeBuffer m_noiseBuffer;
        [Inject]PerlinAPI _perlinAPI;

        [Inject] public void Construct()
        {
            m_perlinNoise = Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise2D"));
            m_noiseBuffer = new ComputeBuffer((TerrainConfig.chunkSize + TerrainConfig.kernelNumber) * (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) * 6, sizeof(float) * 3);
            m_perlinNoise.SetInt("_Width", TerrainConfig.chunkSize + TerrainConfig.kernelNumber);
            m_perlinNoise.SetTexture(_perlinAPI.p2d.type, "_PermTable1D", _perlinAPI.perlin.PermutationTable1D);
            m_perlinNoise.SetTexture(_perlinAPI.p2d.type, "_Gradient2D", _perlinAPI.perlin.Gradient2D);
            m_perlinNoise.SetBuffer(_perlinAPI.p2d.type, "_Result", m_noiseBuffer);
            ReloadSettings();
        }

        void ReloadSettings()
        {
            m_perlinNoise.SetInt("_Octaves", _perlinAPI.p2d.octaves);
            m_perlinNoise.SetFloat("_Idk", _perlinAPI.p2d.idk);
            m_perlinNoise.SetFloat("_Gain", _perlinAPI.p2d.gain);
            m_perlinNoise.SetFloat("_Frequency", _perlinAPI.p2d.frequency);
            m_perlinNoise.SetFloat("_Lacunarity", _perlinAPI.p2d.lacunarity);
        }

        public void Generate(Vector3 offset, Action<AsyncGPUReadbackRequest> callback)
        {
            m_perlinNoise.SetFloat("_X", offset.x * TerrainConfig.chunkSize);
            m_perlinNoise.SetFloat("_Y", offset.y * TerrainConfig.chunkSize);
            m_perlinNoise.SetFloat("_Z", offset.z * TerrainConfig.chunkSize);
            m_perlinNoise.Dispatch(_perlinAPI.p2d.type, (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) / TerrainConfig.kernelNumber, (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) / TerrainConfig.kernelNumber, 1);
            AsyncGPUReadback.Request(m_noiseBuffer, callback);
        }

        public class Pool : MonoMemoryPool<TopologyWorker>
        {
        }
    }
}

