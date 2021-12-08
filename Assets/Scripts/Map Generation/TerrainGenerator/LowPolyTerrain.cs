using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyTerrain : MonoBehaviour
{
    private static LowPolyTerrain instance = null;
    private int chunk_size = 64;
    public Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();
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

    [SerializeField] public List<Transform> focusPoints = new List<Transform>();


    public List<Vector3> GenerateChunksAroundFocusPoints(float radius)
    {
        throw new System.NotImplementedException();
        foreach (Transform focusPoint in focusPoints)
        {

        }

    }

    public void GenerateChunk(Vector3 id)
    {
        if (!chunks.ContainsKey(id))
        {
            GameObject chunk = new GameObject();
            chunk.transform.position = new Vector3(id.x * chunk_size, 0, id.y * chunk_size);
            chunk.transform.parent = transform;
            Chunk t = chunk.gameObject.AddComponent<Chunk>();
            chunks.Add(id, t);
            t.terrainReference = this;
            t.size = chunk_size;
            t.offsetX = (int)id.x * chunk_size;
            t.offsetY = (int)id.z * chunk_size;
            t.Init(id);
        }
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

