using System.Collections.Generic;
using UnityEngine;
using Zenject;





public class TerrainV3 : MonoBehaviour, ITerrain
{
    [Inject] ChunkA.Factory _chunkFactory;
    [Inject] IUiDirector uiDirector;
    int chunkSize = 64;

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        chunkSize = 64;
        spawnChunk();
        var b = new GameObject();
        _chunkFactory.Create(b);
    }

    void spawnChunk()
    {

        //transform.Adopt(chunk);
    }
}
