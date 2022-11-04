using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ImprovedPerlinNoiseProject;
using UnityEngine.Rendering;

public class LowPolyTerrain2D : MonoBehaviour
{
    public static LowPolyTerrain2D instance = null;
    public int chunk_size = 100;
    public Dictionary<Vector3, ChunkOld> chunks = new Dictionary<Vector3, ChunkOld>();
    public ObjectPool<PerlinGenerator> perlinGeneratorPool;
    public int seed = 0;
    public bool rebuildOnSeedChange = false;
    public int generationRadius = 2;
    public int despawnRadius = 2;
    public int perlinGeneratorsAmount = 5;

    private int _last_frame_seed = 0;


    #region Unity Methods

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        PreparePools();
        //Thread threadGen = new Thread(() => GenerationThread());
        //threadGen.Start();
    }

    private void Update()
    {
        RebuildOnSeedChange();
        SeedGenerators();
        DespawnFarChunks();
        if (rebuildOnSeedChange)
            AsyncGPUReadback.WaitAllRequests();
    }

    private void OnDestroy()
    {
        ClearPools();
        DestroyEveryChunk();
    }

    #endregion

    #region Main Methods

    private void RebuildOnSeedChange()
    {
        if (rebuildOnSeedChange)
        {
            if (seed != _last_frame_seed)
            {
                PerlinAPI.instance.seed = seed;
                PerlinAPI.instance.ReloadPerlin();
                perlinGeneratorPool.ClearPool();
                perlinGeneratorPool.RepopulatePool();
                DestroyEveryChunk();
            }
            _last_frame_seed = seed;
        }
    }

    public void DestroyEveryChunk()
    {
        foreach (ChunkOld chunk in chunks.Values)
        {
            Destroy(chunk);
        }
        chunks.Clear();
    }

    private void DespawnFarChunks()
    {
        List<Vector3> chunks_to_remove = new List<Vector3>();
        foreach(Vector3 chunkId in chunks.Keys)
        {
            if (Vector3.Distance(chunkId * chunk_size, PlayerAPI.GetPlayerPosition()) > despawnRadius * chunk_size )
            {
                chunks_to_remove.Add(chunkId);
            }
        }
        chunks_to_remove.ForEach(p => Destroy(chunks[p]));
        chunks_to_remove.ForEach(p => chunks.Remove(p));
    }

    private void SeedGenerators()
    {
        List<Vector3> chunksIds = FindChunkIdsAroundAPI.FindChunksIdsAroundSquare(PlayerAPI.GetPlayerPosition(), generationRadius, chunk_size);
        foreach (Vector3 chunkId in chunksIds)
        {
            if (chunkId.y == 0 && !chunks.ContainsKey(chunkId))
            {
                PerlinGenerator perlinGenerator;
                if (perlinGeneratorPool.GetOne(out perlinGenerator))
                {
                    perlinGenerator.Generate(chunkId);
                }
                break;
            }

        }
    }

    private void PreparePools()
    {
        perlinGeneratorPool = new ObjectPool<PerlinGenerator>(perlinGeneratorsAmount);
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
            ChunkOld t = chunk.gameObject.AddComponent<ChunkOld>();
            chunks.Add(id, t);
            //t.terrainReference = this;
            t.BuildInit(id, perlinData);
            return true;
        }
        return false;
    }

    #endregion

}

