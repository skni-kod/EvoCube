using ImprovedPerlinNoiseProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;


namespace EvoCube.MapGeneration
{
    public class MeshTopologyWorker : MonoBehaviour
    {
        public ComputeShader m_perlinNoise;
        private ComputeBuffer m_noiseBuffer;
        private int chunksize = 64;
        public Chunk chunk;
        private static int N = 8;
        public Vector3 chunkId = new Vector3(0, 0, 0);
        /*
        public void Init()
        {
            m_perlinNoise = UnityEngine.Object.Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise2D"));
            m_noiseBuffer = new ComputeBuffer((chunksize + N) * (chunksize + N) * 6, sizeof(float) * 3);
            m_perlinNoise.SetInt("_Width", chunksize + N);
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
            m_perlinNoise.SetFloat("_X", offset.x * chunksize);
            m_perlinNoise.SetFloat("_Y", offset.y * chunksize);
            m_perlinNoise.SetFloat("_Z", offset.z * chunksize);
            m_perlinNoise.Dispatch(PerlinAPI.p2d.type, (chunksize + N) / N, (chunksize + N) / N, 1);
            AsyncGPUReadback.Request(m_noiseBuffer, GetNoiseDataCallback);
            //AsyncGPUReadback.WaitAllRequests();
        }
        */
        public void GetNoiseDataCallback(AsyncGPUReadbackRequest request)
        {
            chunk.BuildMesh(request.GetData<Vector3>().ToArray());
        }

    }
}