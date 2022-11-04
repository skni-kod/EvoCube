using System.Collections.Generic;
using UnityEngine;
using Zenject;





public class TerrainV3 : MonoBehaviour, ITerrain
{
    [Inject] readonly ChunkA.Factory _chunkFactory;
    [Inject] readonly IUiDirector uiDirector;
    int chunkSize = 64;

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        chunkSize = 64;
        spawnChunk();
    }

    void spawnChunk()
    {
        var chunk = new GameObject("Chunk");
        _chunkFactory.Create(chunk);
        transform.Adopt(chunk);
    }
}
