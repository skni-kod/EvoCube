using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;


[System.Serializable]
public class TerrainSettings
{
    [Range(1, 96)]
    public int size = 64;
    public float total_noise_depth = 1;
    public float small_noise_precentage = 0.8f;
    public float small_noise_detail = 7;
    public float medium_noise_precentage = 8;
    public float medium_noise_detail = 60;
    public float big_noise_precentage = 20;
    public float big_noise_detail = 140;
    public Gradient gradient;
    public Color greyRock;
    public Color orangeDirt;
}


public class TerrainGeneration : MonoBehaviour
{
    // Kolejki do obsługi wątków
    private ConcurrentQueue<MeshGenerator> queue = new ConcurrentQueue<MeshGenerator>();
    private ConcurrentQueue<MeshGenerator> generators_queue = new ConcurrentQueue<MeshGenerator>();
    private ConcurrentQueue<Vector2> cam_positions_queue = new ConcurrentQueue<Vector2>();


    // Różne potrzebne rzeczy
    public GameObject spider;
    public GameObject empty_chunk;
    public Transform[] stones;
    public Camera main_camera;
    public int radius_of_generation = 2;
    public bool circle_generation = false;
    public ConcurrentDictionary<Vector2, Vector2> chunkList = new ConcurrentDictionary<Vector2, Vector2>();
    public Vector3 static_offset = new Vector3(2131, 0, 1239);
    private GameObject clone;
    public TerrainSettings terrain_settings;


    void Start()
    {
        UpdateCameraInfo();
        GenerateGenerators();
        Thread thread = new Thread(() => GenerateChunksAround());
        thread.Start();
    }


    void Update()
    {
        UpdateCameraInfo();
        GenerateGenerators();
        GenerateReadyChunkFromQueue();
        UnloadFarChunks();
    }


    void UnloadFarChunks()
    {
        int x = (int)(main_camera.transform.position.x / 64f);
        int y = (int)(main_camera.transform.position.z / 64f);
        List<Vector2> chunksToDelete = new List<Vector2>();
        List<GameObject> childrenToDelete = new List<GameObject>();
        foreach (Vector2 chunk in chunkList.Values)
        {
            if (Mathf.Abs(chunk.x - x) > radius_of_generation || Mathf.Abs(chunk.y - y) > radius_of_generation)
            {
                chunksToDelete.Add(chunk);
            }
        }

        foreach (Vector2 chunk in chunksToDelete)
        {
            Vector2 ignore;
            chunkList.TryRemove(chunk, out ignore);
            Destroy(transform.Find(string.Format("{0}-{1}", chunk.x, chunk.y)).gameObject.GetComponent<MeshFilter>().mesh);
            Destroy(transform.Find(string.Format("{0}-{1}", chunk.x, chunk.y)).gameObject);
            Resources.UnloadUnusedAssets();
        }
    }


    void UpdateCameraInfo()
    {
        // Obliczamy obecną pozycję kamery i wkładamy do kolejki
        // Ta funkcja jest zwykła
        int x = (int)(spider.transform.position.x / 64f);
        int z = (int)(spider.transform.position.z / 64f);
        cam_positions_queue.Enqueue(new Vector2(x, z));
    }


    void GenerateGenerators()
    {
        // Sprawdzamy czy kolejka z generatorami dla wątku od generacji ma wystarczająco czystych meshGeneratorów, jeśli nie tworzymy nowy i wkładamy do kolejki
        // Ta funkcja jest zwykła
        if (generators_queue.Count < 5)
        {
            MeshGenerator clear_meshGenerator = ScriptableObject.CreateInstance<MeshGenerator>();
            generators_queue.Enqueue(clear_meshGenerator);
        }
    }


    void CalculateMeshGenerator(int x, int z, MeshGenerator meshGenerator)
    {
        // Obliczamy nowego chunka i wsadzamy do kolejki zeby główny wątek unity mógł go sobie wziąść
        // Ta funkcja jest dla wątku generacji
        meshGenerator.chunk_offset = new Vector3(x * terrain_settings.size, 0, z * terrain_settings.size);
        meshGenerator = SetVariablesOfNewGenerator(meshGenerator);
        meshGenerator.CreateMesh();
        queue.Enqueue(meshGenerator);
    }


    MeshGenerator SetVariablesOfNewGenerator(MeshGenerator meshGenerator)
    {
        // Ustawiamy pola czystego meshgeneratora na takie jak są w terrainie
        // Ta funkcja jest dla wątku generacji
        meshGenerator.static_offset = static_offset;
        meshGenerator.static_offset = static_offset;
        meshGenerator.tSett = terrain_settings;

        return meshGenerator;
    }


    void GenerateReadyChunkFromQueue()
    {
        // Odbieramy wygenerowany meshGenerator z kolejki od wątku generującego, klonujemy wzornik(istniejący pusty chunk) i ustawiamy wszystkie jego pola według meshGeneratora
        // Ta fukcja jest zwykła
        MeshGenerator new_mesh_generator;
        if (queue.TryDequeue(out new_mesh_generator))
        {
            clone = Instantiate(empty_chunk, new_mesh_generator.chunk_offset, Quaternion.identity);
            clone.transform.SetParent(this.transform);
            clone.transform.name = string.Format("{0}-{1}", new_mesh_generator.chunk_offset.x / terrain_settings.size, new_mesh_generator.chunk_offset.z / terrain_settings.size);
            Mesh new_mesh = clone.GetComponent<MeshFilter>().mesh;
            new_mesh.Clear();
            new_mesh.vertices = new_mesh_generator.vertices;
            new_mesh.triangles = new_mesh_generator.triangles;
            new_mesh.colors = new_mesh_generator.colors;
            new_mesh.RecalculateNormals();
            clone.GetComponent<MeshCollider>().sharedMesh = new_mesh;
            //clone.GetComponent<SerializableMesh>().SaveMesh(new_mesh, clone.transform.name);

        }
    }


    void GenerateChunksAround()
    {
        // Sprawdzamy czy jest nowa pozycja kamery w kolejce z pozycjami kamery. jeśli jest sprawdzamy w liście z wygenerowanymi chunkami, czy są w zasięgu jakieś niewygenerowane pola,
        //jeśli są, sprawdzamy czy jest dostępny czysty meshGenerator z kolejki z generatorami. Jeśli jest, bierzemy go i obliczamy.
        // Ta funkcja jest dla wątku generacji
        while (true)
        {
            Vector2 cam_pos;
            if (cam_positions_queue.TryDequeue(out cam_pos))
            {
                for (int i = 1 - radius_of_generation; i < radius_of_generation; i++)
                {
                    for (int j = 1 - radius_of_generation; j < radius_of_generation; j++)
                    {
                        if (circle_generation)
                        {
                            if ((Mathf.Pow(i, 2) + Mathf.Pow(j, 2)) <= Mathf.Pow(radius_of_generation, 2))
                            {
                                if (!chunkList.ContainsKey(new Vector2(i + cam_pos.x, cam_pos.y + j)))
                                {
                                    MeshGenerator new_mesh_generator;
                                    if (generators_queue.TryDequeue(out new_mesh_generator))
                                    {
                                        chunkList.TryAdd(new Vector2(i + cam_pos.x, j + cam_pos.y), new Vector2(i + cam_pos.x, j + cam_pos.y));
                                        CalculateMeshGenerator(i + (int)cam_pos.x, j + (int)cam_pos.y, new_mesh_generator);
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (!chunkList.ContainsKey(new Vector2(i + cam_pos.x, cam_pos.y + j)))
                            {
                                MeshGenerator new_mesh_generator;
                                if (generators_queue.TryDequeue(out new_mesh_generator))
                                {
                                    chunkList.TryAdd(new Vector2(i + cam_pos.x, j + cam_pos.y), new Vector2(i + cam_pos.x, j + cam_pos.y));
                                    CalculateMeshGenerator(i + (int)cam_pos.x, j + (int)cam_pos.y, new_mesh_generator);
                                }
                            }
                        }

                    }
                }
            }
        }
    }

}