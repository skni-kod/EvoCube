using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyTerrain : MonoBehaviour
{
    private static LowPolyTerrain instance = null;
    private int chunk_size = 64;
    public Dictionary<int, int> chunks = new Dictionary<int, int>();
    [SerializeField] public Perlin2dSettings p2d;
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void GenerateChunksAround()
    {
        throw new System.NotImplementedException();
    }

    public void GenerateChunk(Vector2 id)
    {
        GameObject chunk = new GameObject();
        chunk.transform.position = new Vector3(id.x * chunk_size, 0, id.y * chunk_size);
        chunk.transform.parent = transform;
        Chunk t = chunk.gameObject.AddComponent<Chunk>();
        t.terrainReference = this;
        t.size = chunk_size;
        t.offsetX = (int)id.x * chunk_size;
        t.offsetY = (int)id.y * chunk_size;
        t.Init();
    }

    public void Start()
    {
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                GenerateChunk(new Vector2(x, y));
            }
        }
    }
}

