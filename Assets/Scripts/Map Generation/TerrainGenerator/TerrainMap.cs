using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapGenSettings : ScriptableObject
{
    [SerializeField] public int chunk_size = 64;
    [SerializeField] public float frequency = 0.02f;
    [SerializeField] public float lacunarity = 2f;
    [SerializeField] public float gain = 0.5f;
    [SerializeField] public int seed = 0;
    [SerializeField] public int octaves = 4;
    [SerializeField] public float resolution = 1f;
    [SerializeField] public float idk = 1;
    [SerializeField] public int type = 0;
}

public class ChunksHolder
{

}

public class ChunkGenerator : IPooledObject
{
    #region ComputeBuffers
    private ComputeBuffer m_noiseBuffer;
    private ComputeBuffer m_meshBuffer;
    private ComputeBuffer m_cubeEdgeFlags;
    private ComputeBuffer m_triangleConnectionTable;
    #endregion
    #region ComputeShaders
    private ComputeShader m_perlinNoise;
    private ComputeShader m_marchingCubes;
    private ComputeShader m_normals;
    #endregion
    private Vector3 offset = Vector3.zero;
    private MapGenSettings mapGenSettings;


    public void Init()
    {
        m_perlinNoise = Object.Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise3D"));
        m_marchingCubes = Object.Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/MarchingCubes"));
        m_normals = Object.Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/Normals"));
        InitializeBuffers();
        ReloadSettings();
    }

    private void InitializeBuffers()
    {
        //There are 8 threads run per group so N must be divisible by 8.
        if (mapGenSettings.chunk_size % 8 != 0)
            throw new System.ArgumentException("Chunk size must be divisible be 8");

        //Holds the voxel values, generated from perlin noise.
        m_noiseBuffer = new ComputeBuffer((int)Mathf.Pow(mapGenSettings.chunk_size, 3), sizeof(float));
        m_meshBuffer = new ComputeBuffer((int)Mathf.Pow(mapGenSettings.chunk_size, 3) * 5 * 3, sizeof(float) * 7);
    }

    public void OnRelease()
    {
        m_noiseBuffer.Release();
        m_meshBuffer.Release();
        m_cubeEdgeFlags.Release();
        m_triangleConnectionTable.Release();
    }

    public void Reload()
    {

    }

    private void ReloadSettings()
    {

    }
}

public class ChunkBuilder
{
    public ChunkBuilder()
    {

    }

    public void InitializeGeneration()
    {

    }

}


public class TerrainMap : MonoBehaviour
{
    public static TerrainMap instance;
    private MapGenSettings settings;
    private ChunksHolder chunks;
    private ChunkBuilder chunkBuilder;
    private ObjectPool<ChunkGenerator> chunkGenerators;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void FindChunksIdsToGenerate()
    {

    }

    void GiveChunksIdsToChunkGenerators()
    {

    }
    
    void Update()
    {
        FindChunksIdsToGenerate();

    }
}
