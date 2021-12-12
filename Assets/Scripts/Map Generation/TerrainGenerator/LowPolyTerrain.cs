using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ImprovedPerlinNoiseProject;

public class LowPolyTerrain : MonoBehaviour
{
    public static LowPolyTerrain instance = null;
    public int chunk_size = 100;
    public Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();
    [SerializeField] public Perlin2dSettings p2d;
    private GPUPerlinNoise perlin;
    ObjectPool<PerlinGenerator> perlinGeneratorPool = new ObjectPool<PerlinGenerator>(5);
    private ConcurrentQueue<MeshBuilder> queue_readyMeshBuilders = new ConcurrentQueue<MeshBuilder>();


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
        Thread threadGen = new Thread(() => GenerationThread());
        threadGen.Start();
    }

    #region Main Methods

    public bool GenerateChunk(Vector3 id)
    {
        if (!chunks.ContainsKey(id))
        {
            GameObject chunk = new GameObject();
            chunk.transform.position = new Vector3(id.x * chunk_size, id.y * chunk_size, id.z * chunk_size);
            chunk.transform.parent = transform;
            Chunk t = chunk.gameObject.AddComponent<Chunk>();
            chunks.Add(id, t);
            t.terrainReference = this;
            t.size = chunk_size;
            t.Init(id);
            return true;
        }
        return false;
    }

    #endregion

    #region Thread Methods

    private void GenerationThread()
    {
        List<Vector3> spawn_later = new List<Vector3>();
        for (; ; )
        {
            List<Vector3> chunksIds = FindChunkIdsAroundAPI.FindChunksIdsAroundSquare(PlayerAPI.GetPlayerPosition(), 2);
            foreach(Vector3 chunkId in chunksIds)
            {
                PerlinGenerator perlinGenerator;
                if (perlinGeneratorPool.container.TryDequeue(out perlinGenerator))
                {
                    perlinGenerator.Generate(chunkId);
                }
                else
                {
                    if (!spawn_later.Contains(chunkId))
                        spawn_later.Add(chunkId);
                }
            }
            Thread.Sleep(50);
        }
    }

    #endregion
}

