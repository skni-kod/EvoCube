using ImprovedPerlinNoiseProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvoCube.MapGeneration
{
    public class PerlinAPI : MonoBehaviour
    {
        public static GPUPerlinNoise perlin;
        public static GPUPerlinNoise perlin3D;
        public static Perlin2dSettings p2d;
        public static bool initialised = false;

        private void Awake()
        {
            p2d = ScriptableObject.CreateInstance<Perlin2dSettings>();
            initialised = true;
            ReloadPerlin();
            ReloadPerlin3D();
        }
        public void ReloadPerlin()
        {
            perlin = new GPUPerlinNoise(TerrainConfig.seed);
            perlin.LoadResourcesFor2DNoise();
        }
        public static void ReloadPerlin3D()
        {
            perlin3D = new GPUPerlinNoise(TerrainConfig.seed);
            perlin3D.LoadResourcesFor3DNoise();
        }
        public static float[] GPUPerlin2D(int size, Vector3 offset)
        {
            int corrected_size = size;
            while (corrected_size % TerrainConfig.kernelNumber != 0)
                corrected_size++;
            //float[] map = GPUPerlin2dMAP(offset, corrected_size);
            float[] new_map = new float[size * size];
            //int idx = 0;
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


    }
}

