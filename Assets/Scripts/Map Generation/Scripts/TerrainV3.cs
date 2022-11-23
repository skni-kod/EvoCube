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

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            spawnChunk(new Vector3(0, 0, 0));
            spawnChunk(new Vector3(1, 0, 0));
            spawnChunk(new Vector3(0, 0, 1));
            spawnChunk(new Vector3(1, 0, 1));

            spawnChunk(new Vector3(2, 0, 0));
            spawnChunk(new Vector3(3, 0, 0));
            spawnChunk(new Vector3(2, 0, 1));
            spawnChunk(new Vector3(3, 0, 1));

            spawnChunk(new Vector3(0, 0, 2));
            spawnChunk(new Vector3(1, 0, 2));
            spawnChunk(new Vector3(0, 0, 3));
            spawnChunk(new Vector3(1, 0, 3));

            spawnChunk(new Vector3(2, 0, 2));
            spawnChunk(new Vector3(3, 0, 2));
            spawnChunk(new Vector3(2, 0, 3));
            spawnChunk(new Vector3(3, 0, 3));
        }

        void spawnChunk(Vector3 id)
        {
            var chunkObject = new GameObject("Chunk" + id.ToString());
            var chunk = _chunkFactory.Create(chunkObject);
            transform.Adopt(chunkObject);
            chunkObject.transform.position = id * TerrainConfig.chunkSize;
            TopologyWorker worker = _topologyWorkerPool.Spawn();
            //worker.Init();
            worker.Generate(id, chunk.BuildMeshCallback);
        }
    }
}


