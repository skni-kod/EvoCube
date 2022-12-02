using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ImprovedPerlinNoiseProject;
using System;
using UnityEngine.Rendering;

namespace EvoCube.MapGeneration
{
    public class GPUMeshDataGenerator : MonoBehaviour
    {
        public ComputeShader perlinNoise;
        public ComputeBuffer noiseBuffer;
        GPUPerlinNoise GPUPerlin;
        public bool finished = false;
        public Mesh mesh;
        [SerializeField]public Perlin2dSettings p2d;


        public void Awake()
        {
            Init();
            Generate(transform.position / TerrainConfig.chunkSize, BuildMeshCallback);
        }
        public void Init()
        {
            GPUPerlin = GPUPerlinNoise.Get2DNoise(TerrainConfig.seed);

            perlinNoise = Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise2DExperimental"));
            noiseBuffer = new ComputeBuffer((TerrainConfig.chunkSize + TerrainConfig.kernelNumber) * (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) * 6, sizeof(float) * 3);
            perlinNoise.SetInt("_Width", TerrainConfig.chunkSize + TerrainConfig.kernelNumber);
            perlinNoise.SetTexture(0, "_PermTable1D", GPUPerlin.PermutationTable1D);
            perlinNoise.SetTexture(0, "_Gradient2D", GPUPerlin.Gradient2D);
            perlinNoise.SetBuffer(0, "_Result", noiseBuffer);
            ReloadSettings();
        }
        void ReloadSettings()
        {
            perlinNoise.SetInt("_Octaves", p2d.octaves);
            perlinNoise.SetFloat("_Idk", p2d.idk);
            perlinNoise.SetFloat("_Gain", p2d.gain);
            perlinNoise.SetFloat("_Frequency", p2d.frequency);
            perlinNoise.SetFloat("_Lacunarity", p2d.lacunarity);
        }

        public void Generate(Vector3 offset, Action<AsyncGPUReadbackRequest> callback)
        {
            finished = false;
            perlinNoise.SetFloats("_Position", new float[] { offset.x * TerrainConfig.chunkSize, offset.z * TerrainConfig.chunkSize });//offset.x * TerrainConfig.chunkSize);
            perlinNoise.Dispatch(0, (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) / TerrainConfig.kernelNumber, (TerrainConfig.chunkSize + TerrainConfig.kernelNumber) / TerrainConfig.kernelNumber, 1);
            AsyncGPUReadback.Request(noiseBuffer, callback);
        }

        public void BuildMeshCallback(AsyncGPUReadbackRequest request)
        {
            if (!mesh)
            {
                mesh = new Mesh();
            }
            mesh.indexFormat = IndexFormat.UInt32;
            mesh.vertices = MeshAPI.ResizePerlinVerticesDown(request.GetData<Vector3>().ToArray());
            mesh.triangles = MeshAPI.CalculateTrianglesFlat(TerrainConfig.chunkSize);
            mesh.RecalculateNormals();
            finished = true;
        }


    }
}



