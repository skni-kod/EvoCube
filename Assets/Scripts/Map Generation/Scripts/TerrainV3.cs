using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace EvoCube.MapGeneration
{
    public class TerrainV3 : MonoBehaviour, ITerrain
    {
        [Inject] readonly Chunk.Factory _chunkFactory;
        [Inject] readonly IUiDirector uiDirector;
        [Inject] readonly TopologyWorker.Pool _topologyWorkerPool;
        Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();
        int _timer = 0;

        Queue<MeshData> queueDataMeshses = new Queue<MeshData>();


        private void findChunkIdsAroundPoint(Vector3 point)
        {

        }

        public void Start()
        {
            Initialize();
        }

        public void FixedUpdate()
        {
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
            //regenerateChunk(Vector3.zero);
        }

        public void Initialize()
        {
            spawnChunk(Vector3.zero);
        }

        void regenerateChunk(Vector3 id)
        {
            if (chunks.ContainsKey(id))
            {
                Chunk chunk = chunks[id];
                TopologyWorker worker = _topologyWorkerPool.Spawn();
                worker.HardReload();
                worker.Generate(new Vector3((int)(Random.value * 30), (int)(Random.value * 30), (int)(Random.value * 30)), chunk.RegenerateMeshCallback);
            }
            else
            {
                spawnChunk(id);
            }
        }

        void spawnChunk(Vector3 id)
        {
            var chunkObject = new GameObject("Chunk" + id.ToString());
            var chunk = _chunkFactory.Create(chunkObject);
            transform.Adopt(chunkObject);
            chunkObject.transform.position = id * TerrainConfig.chunkSize;
            TopologyWorker worker = _topologyWorkerPool.Spawn();
            worker.Generate(id, chunk.BuildMeshCallback);
            chunks.Add(id, chunk);
        }
    }
}


