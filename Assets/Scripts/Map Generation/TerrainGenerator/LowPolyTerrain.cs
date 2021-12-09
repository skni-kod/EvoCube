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
    private int chunk_size = 100;
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
    [SerializeField] public float generation_refresh_rate = 0.2f;
    [SerializeField] public float chunk_spawn_refresh_rate = 0.01f;

    private ConcurrentQueue<MeshGenerator> queue = new ConcurrentQueue<MeshGenerator>();
    private ConcurrentQueue<MeshGenerator> generators_queue = new ConcurrentQueue<MeshGenerator>();

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
            center = new Vector3(Mathf.RoundToInt(center.x), Mathf.RoundToInt(center.y), Mathf.RoundToInt(center.z));
            int x_range = 3;
            int y_range = 3;
            int z_range = 3;

            for (int x = -x_range; x < x_range; x++)
            {
                for (int z = -z_range; z < z_range; z++)
                {
                    for (int y = -y_range; y < y_range; y++)
                    {
                        Vector3 p = new Vector3(x + center.x, y + center.y, z + center.z);
                        if (!chunks.ContainsKey(p) && p.y == 0)
                        {
                            ids_to_generate.Add(p);
                        }
                    }
                }
            }
        }
    }


    public bool GenerateChunk(Vector3 id)
    {
        if (!chunks.ContainsKey(id))
        {
            GameObject chunk = new GameObject();
            chunk.transform.position = new Vector3(id.x * chunk_size, id.y * chunk_size, id.z * chunk_size);
            chunk.transform.parent = transform;
            Chunk t = chunk.gameObject.AddComponent<Chunk>();
            chunks.Add(id, t);
            t.terrainReference = this;
            t.size = chunk_size;
            t.offsetX = (int)id.x * chunk_size;
            t.offsetY = (int)id.y * chunk_size;
            t.offsetZ = (int)id.z * chunk_size;
            t.Init(id);
            return true;
        }
        return false;
    }

    public void Start()
    {
        //StartCoroutine(GenerationChecksCouroutine());
        //StartCoroutine(MainGenerationCouroutine());
    }

    private void Update()
    {

        _generateChunksAroundPointsOfFocus();
        bool test = false;
        while (!test && ids_to_generate.Count > 0)
        {
            test = GenerateChunk(ids_to_generate[0]);
            ids_to_generate.RemoveAt(0);
        }

    }

}

