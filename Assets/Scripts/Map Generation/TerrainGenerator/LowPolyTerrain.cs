using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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
    List<Vector3> ids_to_generate = new List<Vector3>();
    [SerializeField] public float radius_of_generation = 200f;
    [SerializeField] public float generation_refresh_rate = 10f;
    [SerializeField] public float chunk_spawn_refresh_rate = 60f;

    private IEnumerator GenerationChecksCouroutine()
    {
        for(;;)
        {
            _generateChunksAroundPointsOfFocus();
            yield return new WaitForSeconds(1f / generation_refresh_rate);
        }
    }

    private IEnumerator MainGenerationCouroutine()
    {
        for (;;)
        {
            if (ids_to_generate.Count > 0)
            {
                GenerateChunk(ids_to_generate[0]);
                ids_to_generate.RemoveAt(0);
            }
            yield return new WaitForSeconds(1f / chunk_spawn_refresh_rate);
        }
    }

    private void _generateChunksAroundPointsOfFocus()
    {
        foreach (Transform focusPoint in focusPoints)
        {
            Vector3 center = focusPoint.position / (float)chunk_size;
            center = new Vector3(Mathf.RoundToInt(center.x), Mathf.RoundToInt(center.y), Mathf.RoundToInt(center.z)) * (float)chunk_size;

            int x_range = (int)(radius_of_generation / (float)chunk_size) * 2;
            int y_range = (int)(radius_of_generation / (float)chunk_size) * 2;
            int z_range = (int)(radius_of_generation / (float)chunk_size) * 2;

            for (int x = 0; x - (x_range/2) < x_range; x++)
            {
                for (int z = 0; z - (z_range / 2) < z_range; z++)
                {
                    for (int y = 0; y - (y_range / 2) < y_range; y++)
                    {
                        if (Mathf.Pow(x-center.x,2) + Mathf.Pow(y - center.y,2) + Mathf.Pow(z - center.z,2) < Mathf.Pow(radius_of_generation / (float)chunk_size, 2))
                            ids_to_generate.Add(new Vector3(x, y, z));
                    }
                }
            }
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
        StartCoroutine(GenerationChecksCouroutine());
        StartCoroutine(MainGenerationCouroutine());
    }
}

