using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] GameObject chunk_prefab;
    private static Terrain instance = null;
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

    private int chunk_size = 128;
    public Dictionary<int, int> chunks = new Dictionary<int, int>();

    private void Update()
    {
        for (int i =0; i<transform.GetChildCount();i++)
        {
            transform.GetChild(i).GetComponent<MarchinCubeTest>().offsetX += 1;
        }
    }
    public void GenerateChunksAround()
    {

    }

    public void GenerateChunk(Vector2 id)
    {
        GameObject chunk = Instantiate(chunk_prefab, new Vector3(id.x * chunk_size, 0, id.y * chunk_size), Quaternion.identity, transform);
        MarchinCubeTest t = chunk.GetComponent<MarchinCubeTest>();
        t.size = chunk_size;
        t.offsetX = (int)id.x * chunk_size;
        t.offsetY = (int)id.y * chunk_size;
    }

    public void Start()
    {
        GenerateChunk(new Vector2(0, 0));
        GenerateChunk(new Vector2(1, 0));
        GenerateChunk(new Vector2(0, 1));
        GenerateChunk(new Vector2(1, 1));
        GenerateChunk(new Vector2(1, 2));
        GenerateChunk(new Vector2(2, 1));
        GenerateChunk(new Vector2(2, 2));
        GenerateChunk(new Vector2(0, 2));
        GenerateChunk(new Vector2(2, 0));
    }
}
