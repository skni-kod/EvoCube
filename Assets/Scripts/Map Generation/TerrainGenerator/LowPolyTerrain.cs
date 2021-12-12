using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ImprovedPerlinNoiseProject;
using UnityEngine.Rendering;

public class LowPolyTerrain : MonoBehaviour
{
    public static LowPolyTerrain instance = null;
    public int chunk_size = 100;
    public Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();
    private GPUPerlinNoise perlin;
    public ObjectPool<PerlinGenerator> perlinGeneratorPool;
    private List<PerlinGenerator> queue_PerlinsWaitingForGPU = new List<PerlinGenerator>();
    public List<Vector3> spawn_later = new List<Vector3>();

    #region Unity Methods

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        perlin = new GPUPerlinNoise(PerlinAPI.seed);
        perlin.LoadResourcesFor2DNoise();
    }

    public void Start()
    {
        PreparePools();
        //Thread threadGen = new Thread(() => GenerationThread());
        //threadGen.Start();
    }

    private void Update()
    {
        SeedGenerators();
        //CheckGenerators();
        DespawnFarChunks();
    }

    private void OnDestroy()
    {
        ClearPools();
    }

    #endregion

    #region Main Methods

    /*private void CheckGenerators()
    {
        List<PerlinGenerator> to_return = new List<PerlinGenerator>();
        foreach (PerlinGenerator gen in queue_PerlinsWaitingForGPU)
        {
            if (true)
            {
                GenerateChunk(gen.chunkId, gen.verts);
                to_return.Add(gen);
            }
        }

        foreach (PerlinGenerator gen in to_return)
        {
            perlinGeneratorPool.ReturnIntoPool(gen);
            queue_PerlinsWaitingForGPU.Remove(gen);
        }

    }*/

    private void DespawnFarChunks()
    {
        List<Vector3> chunks_to_remove = new List<Vector3>();
        foreach(Vector3 chunkId in chunks.Keys)
        {
            if (Vector3.Distance(chunkId * chunk_size, PlayerAPI.GetPlayerPosition()) > 600 )
            {
                chunks_to_remove.Add(chunkId);
            }
        }
        chunks_to_remove.ForEach(p => chunks[p].MyDestroy());
        chunks_to_remove.ForEach(p => chunks.Remove(p));
    }

    private void SeedGenerators()
    {
        List<Vector3> chunksIds = FindChunkIdsAroundAPI.FindChunksIdsAroundSquare(PlayerAPI.GetPlayerPosition(), 9);
        foreach (Vector3 chunkId in chunksIds)
        {
            if (chunkId.y == 0)
            {
                PerlinGenerator perlinGenerator;
                if (perlinGeneratorPool.GetOne(out perlinGenerator))
                {
                    perlinGenerator.Generate(chunkId);
                    //perlinGeneratorPool.ReturnIntoPool(perlinGenerator);
                    //queue_PerlinsWaitingForGPU.Add(perlinGenerator);
                }
                else
                {
                    if (!spawn_later.Contains(chunkId) && !chunks.ContainsKey(chunkId) && false)
                        spawn_later.Add(chunkId);
                }
            }

        }
    }

    private void PreparePools()
    {
        perlinGeneratorPool = new ObjectPool<PerlinGenerator>(1);
    }

    private void ClearPools()
    {
        perlinGeneratorPool.ClearPool();
    }

    public bool GenerateChunk(Vector3 id, Vector3[] perlinData)
    {
        if (!chunks.ContainsKey(id))
        {
            GameObject chunk = new GameObject();
            chunk.transform.position = new Vector3(id.x * chunk_size, id.y * chunk_size, id.z * chunk_size);
            chunk.transform.parent = transform;
            Chunk t = chunk.gameObject.AddComponent<Chunk>();
            chunks.Add(id, t);
            t.terrainReference = this;
            t.BuildInit(id, perlinData);
            return true;
        }
        return false;
    }

    #endregion

}

