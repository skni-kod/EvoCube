using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyTerrain : MonoBehaviour
{
    [SerializeField] GameObject chunk_prefab;
    private static LowPolyTerrain instance = null;
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

    private int chunk_size = 64;
    public Dictionary<int, int> chunks = new Dictionary<int, int>();

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<TestMesh>().offsetX += 1;
        }
    }
    public void GenerateChunksAround()
    {

    }

    public void GenerateChunk(Vector2 id)
    {
        GameObject chunk = Instantiate(chunk_prefab, new Vector3(id.x * chunk_size, 0, id.y * chunk_size), Quaternion.identity, transform);
        TestMesh t = chunk.GetComponent<TestMesh>();
        t.size = chunk_size;
        t.offsetX = (int)id.x * chunk_size;
        t.offsetY = (int)id.y * chunk_size;
    }

    public void Start()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                GenerateChunk(new Vector2(x, y));
            }
        }
    }
}

