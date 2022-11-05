using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TRM = TerrainResourceManager;





public class TerrainV3 : MonoBehaviour, ITerrain
{
    [Inject] readonly ChunkA.Factory _chunkFactory;
    [Inject] readonly IUiDirector uiDirector;
    [Inject] readonly TRM.TopologyWorker.Pool _topologyWorkerPool;



    /*public void RemoveFoo()
    {
        var foo = _topologyWorkers[0];
        _topologyWorkerPool.Despawn(foo);
        _topologyWorkers.Remove(foo);
    }*/

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        spawnChunk(new Vector3(0, 0, 0));
    }

    void spawnChunk(Vector3 id)
    {
        var chunkObject = new GameObject("Chunk");
        var chunk = _chunkFactory.Create(chunkObject);
        transform.Adopt(chunkObject);
        TRM.TopologyWorker worker = _topologyWorkerPool.Spawn();
        worker.Init();
        worker.Generate(id, chunk.BuildMeshCallback);
    }
}
