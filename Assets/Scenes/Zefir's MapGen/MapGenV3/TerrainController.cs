using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TerrainController : MonoBehaviour
{
    private Dictionary<Vector3, Chunk> _chunks = new Dictionary<Vector3, Chunk>();
    private Chunk.Pool _chunkPool;

    [Inject]
    public void Construct(Chunk.Pool chunkPool)
    {
        _chunkPool = chunkPool;
    }

    public void GenerateChunk(Vector3 idd)
    {
        Chunk chunk = _chunkPool.Spawn();
        chunk.transform.SetParent(transform);
        _chunks[idd] = chunk;
    }

    public void RemoveChunk(Vector3 idd)
    {
        if (_chunks.ContainsKey(idd))
        {
            Chunk chunk = _chunks[idd];
            _chunkPool.Despawn(chunk);
            _chunks.Remove(idd);
        }

            
    }

    private void Start()
    {
        GenerateChunk(Vector3.zero);
    }

}
