using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TerrainSubscriber : MonoBehaviour
{

}

public class ChunkV3 : MonoBehaviour, IChunk
{

}

public class TerrainV3 : MonoBehaviour, ITerrain
{
    [Inject][SerializeField] IUiDirector uiDirector;
    private List<TerrainSubscriber> terrainSubscribers = new List<TerrainSubscriber>();
    private int chunkSize = 64;
    

    public void Initialize()
    {
        chunkSize = 64;
        spawnChunk();
    }

    private void spawnChunk()
    {
        GameObject chunk = new GameObject();
        transform.Adopt(chunk);
        
    }


}
