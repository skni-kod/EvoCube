using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerationTester : MonoBehaviour
{
    public Dictionary<int, int> chunks = new Dictionary<int, int>();
    [SerializeField] public Perlin2dSettings p2d;
    [SerializeField] public Material material;
    [SerializeField] public int chunk_size;
    [SerializeField] public Vector2 map_size;
    [SerializeField] public Vector2 scrollingSpeed;
    public void GenerateChunk(Vector2 id)
    {
        GameObject chunk = new GameObject();
        chunk.transform.position = new Vector3(id.x * chunk_size, 0, id.y * chunk_size);
        chunk.transform.parent = transform;
        ChunkGenTesting t = chunk.gameObject.AddComponent<ChunkGenTesting>();
        t.terrainReference = this;
        t.size = chunk_size;
        t.offsetX = (int)id.x * chunk_size;
        t.offsetY = (int)id.y * chunk_size;
        t.Init();
    }

    public void Start()
    {
        for (int x = 0; x < map_size.x; x++)
        {
            for (int y = 0; y < map_size.y; y++)
            {
                GenerateChunk(new Vector2(x, y));
            }
        }
    }
}
