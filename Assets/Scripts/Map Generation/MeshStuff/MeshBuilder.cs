using ImprovedPerlinNoiseProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MeshBuilder
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector3 id;
    public int chunk_size;
    ComputeBuffer m_noiseBuffer;
    GPUPerlinNoise perlin;

    public MeshBuilder(Vector3 idd, int chunk_sizee)
    {
        id = idd;
        chunk_size = chunk_sizee;
        //this.perlin = perlin;
       // m_noiseBuffer = new ComputeBuffer(chunk_sizee * chunk_sizee, sizeof(float));
    }


    #region Main Methods

    public GameObject ExtractChunk(LowPolyTerrain2D terrain_ref)
    {
        if (terrain_ref.chunks.ContainsKey(id))
            return null;

        GameObject chunk = new GameObject();
        chunk.transform.position = new Vector3(id.x * chunk_size, id.y * chunk_size, id.z * chunk_size);
        chunk.transform.parent = terrain_ref.transform;
        ChunkOld t = chunk.gameObject.AddComponent<ChunkOld>();
        terrain_ref.chunks.Add(id, t);
        t.id = id;
        //t.BuildInit(vertices, triangles);
        return chunk;

    }
    #endregion

}
