using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;


namespace EvoCube.MapGeneration
{
    public class ChunkSystem
    {

    }

    public class TerrainV3 : MonoBehaviour, ITerrain
    {
        Transform targetForGeneration;
        [Inject] readonly Chunk.Factory _chunkFactory;
        [Inject] readonly IUiDirector uiDirector;
        [Inject] readonly TopologyWorker.Pool _topologyWorkerPool;
        Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();
        ConcurrentQueue<Chunk> queue = new ConcurrentQueue<Chunk>();
        int _timer = 0;
        Thread genThread;


        public void SetTargetForGeneration(Transform target)
        {
            targetForGeneration = target;
        }

        void startGenerationThreads()
        {
            genThread = new Thread(() => generationThread());
            genThread.Start();
        }

        private List<Vector3> findChunkIdsAroundPoint(Vector3 point, int range)
        {
            List<Vector3> ids = new List<Vector3>();
            Vector3 reducedPos = point / TerrainConfig.chunkSize;
            reducedPos = new Vector3(Mathf.Floor(reducedPos.x), Mathf.Floor(reducedPos.y), Mathf.Floor(reducedPos.z));
            for (int x = -range; x < range; x++)
            {
                for(int z = -range; z < range; z++)
                {
                    Vector3 id = new Vector3(x, 0, z) + reducedPos;
                    id.y = 0;
                    if (!chunks.ContainsKey(id))
                        ids.Add(id);
                }
            }
            return ids;
        }

        void generationThread()
        {
            while(true)
            {
                generationPipeline();

                Thread.Sleep(300);
            }

        }

        void generationPipeline()
        {
            List<Vector3> ids = findChunkIdsAroundPoint(targetForGeneration.position, 3);
            foreach(Vector3 id in ids)
            {
                spawnChunk(id);
            }

        }

        public void Start()
        {
            Initialize();
        }

        public void FixedUpdate()
        {
            generationPipeline();
            if (_timer >= 60)
            {
                everySecondUpdate();
                _timer = 0;
            }
            else
            {
                _timer++;
            }
        }

        private void everySecondUpdate()
        {
            
        }

        public void Initialize()
        {
            
        }

        void spawnChunk(Vector3 id)
        {
            var chunkObject = new GameObject("Chunk" + id.ToString());
            var chunk = _chunkFactory.Create(chunkObject);
            transform.Adopt(chunkObject);
            chunk.Id = id;
            chunkObject.transform.position = id * TerrainConfig.chunkSize;
            TopologyWorker worker = _topologyWorkerPool.Spawn();
            worker.Generate(id, chunk.BuildMeshCallback);
            chunks.Add(id, chunk);
        }
    }
}


