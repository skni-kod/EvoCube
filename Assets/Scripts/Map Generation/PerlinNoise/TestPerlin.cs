using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using ImprovedPerlinNoiseProject;

namespace MarchingCubesGPUProject
{
    [RequireComponent(typeof(MeshFilter))]
    public class TestPerlin : MonoBehaviour
    {
        const int N = 64;
        public int test2 = 5;
        public int m = 1;
        public float gain = 0.5f;
        public float frequency = 0.02f;
        public float lacunarity = 0.02f;
        public float offsetX;
        public float offsetY;
        public int m_seed = 120;

        public ComputeShader m_perlinNoise;

        Mesh mesh;

        ComputeBuffer m_noiseBuffer;

        GPUPerlinNoise perlin;

        private void Start()
        {
            mesh = new Mesh();

        }
        private void Update()
        {
            Generate();
        }

        private float[] RenderInShader()
        {

            if (N % 8 != 0)
            {
                //There are 8 threads run per group so N must be divisible by 8.
                throw new System.ArgumentException("N must be divisible be 8");
            }

            //Holds the voxel values, generated from perlin noise.
            m_noiseBuffer = new ComputeBuffer((N) * (N), sizeof(float));

            //Make the perlin noise, make sure to load resources to match shader used.
            perlin = new GPUPerlinNoise(m_seed);
            perlin.LoadResourcesFor2DNoise();

            //Make the voxels.
            m_perlinNoise.SetInt("_Width", N);
            m_perlinNoise.SetFloat("_Frequency", frequency);
            m_perlinNoise.SetFloat("_Lacunarity", lacunarity);
            m_perlinNoise.SetFloat("_X", offsetX);
            m_perlinNoise.SetFloat("_Y", offsetY);
            m_perlinNoise.SetFloat("_Gain", gain);
            m_perlinNoise.SetTexture(0, "_PermTable1D", perlin.PermutationTable1D);
            m_perlinNoise.SetTexture(0, "_Gradient2D", perlin.Gradient2D);
            m_perlinNoise.SetBuffer(0, "_Result", m_noiseBuffer);

            m_perlinNoise.Dispatch(0, N / 8, N / 8, 1);

            //Get the data out of the buffer.
            float[] verts = new float[(N) * (N)];
            Debug.Log(verts.Length);
            m_noiseBuffer.GetData(verts);
            m_noiseBuffer.Release();
            return verts;
        }
        
        private void Generate()
        {
            if (N % 8 != 0)
            {
                //There are 8 threads run per group so N must be divisible by 8.
                throw new System.ArgumentException("N must be divisible be 8");
            }

            float[] verts = RenderInShader();

            mesh.Clear();
            mesh.vertices = VerticesFlat(verts, N-1);
            mesh.triangles = TrianglesFlat(verts, N-1);
            mesh.RecalculateNormals();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        /*private void Generate3()
        {
            mesh.name = "Procedural Grid";
            Vector3[] vertices = new Vector3[(N + 1) * (N + 1) * 6];
            int[] triangles = new int[(N + 0) * (N + 0) * 6];
            for (int i = 0, y = 0; y <= N; y++)
            {
                for (int x = 0; x <= N; x++, i++)
                {
                    for (int g = 0; g < 6; g++)
                    {
                        vertices[i+g] = new Vector3(x, y);
                    }
                }
            }
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    triangles[(x + y * N) + 0] = (x * 6) + (y * 6 * (N + 1));
                    triangles[(x + y * N) + 1] = 86;//(x * 6) + (y * 6 * (N + 1)) + 6;
                    triangles[(x + y * N) + 2] = 2;//(x * 6) + (y * 6 * (N + 1)) + 6*(N+1);

                    triangles[(x + y * N) + 3] = 3;//(x * 6) + (y * 6 * (N + 1)) + 6;
                    triangles[(x + y * N) + 4] = 4;// (x * 6) + (y * 6 * (N + 1)) + 6 + 6*(N+1);
                    triangles[(x + y * N) + 5] = 5;// (x * 6) + (y * 6 * (N + 1)) + 6 * (N + 1);
                }
            }


            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();


            GetComponent<MeshFilter>().mesh = mesh;
        }*/

        private Vector3[] VerticesFlat(float[] data, int size)
        {
            //data[x + (y * size)]
            Vector3[] vertices = new Vector3[(size + 1) * (size + 1) * 6];
            for (int y = 0; y <= size; y++)
            {
                for (int x = 0; x <= size; x++)
                {
                    for (int g = 0; g < 6; g++)
                    {
                        vertices[((x * 6) + (y * size * 6)) + g] = new Vector3(x, data[((x + (y * size)))], y);
                    }

                }
            }
            return vertices;
        }

        private Vector3[] Vertices(float[] data, int size)
        {
            //data[x + (y * size)]
            //Mathf.PerlinNoise((float)(x + offsetX) / 10f, (float)(y + offsetY) / 10f)
            Vector3[] vertices = new Vector3[(size + 1) * (size + 1) * 6];
            for (int y = 0; y <= size; y++)
            {
                for (int x = 0; x <= size; x++)
                {
                    for (int g = 0; g <= m; g++)
                    {
                        vertices[x + y * (size + 1) + g] = new Vector3(x, data[x + (y * (size + 1))] * 10, y);
                    }
                }
            }
            return vertices;
        }

        private int[] TrianglesFlat(float[] data, int size)
        {
            int idx = 0;
            int[] triangles = new int[(size + 0) * (size + 0) * 12];
            for (int y = 0; y <= size; y++)
            {
                for (int x = 0; x <= size; x++)
                {
                    if (x != size && y != size)
                    {
                        int idd = (x * 6) + (y * 6 * (size + 1));
                        triangles[idx + 0] = idd + 0;
                        triangles[idx + 1] = idd + 0 + 6;
                        triangles[idx + 2] = idd + 0 + (6 * (size + 1));
                        triangles[idx + 3] = idd + 0 + 6;
                        triangles[idx + 4] = idd + 0 + 6 + (6 * (size + 1));
                        triangles[idx + 5] = idd + 0 + (6 * (size + 1));
                        idx += 6;
                    }
                }
            }
            return triangles;
        }

        private int[] Triangles(float[] data, int size)
        {
            int[] triangles = new int[size * size * 6];
            int vert = 0;
            int tris = 0;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    triangles[tris + 0] = (vert*m) + 0;
                    triangles[tris + 1] = (vert*m) + m + (size * m);
                    triangles[tris + 2] = (vert*m) + m;
                    triangles[tris + 3] = (vert*m) + m;
                    triangles[tris + 4] = (vert*m) + m + (size * m);
                    triangles[tris + 5] = (vert*m) + (2*m) + (size * m);
                    vert++;
                    tris += 6;
                }
                vert++;
            }
            return triangles;
        }
    }
}
