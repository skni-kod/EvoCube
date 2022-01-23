using MarchingCubesGPUProject;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public struct Vert
{
    public Vector4 position;
    public Vector3 normal;
};
public static class ChunkBuilder
{ 
    public static GameObject CreateChunk(Vector3 id, Vert[] data)
    {
        GameObject gameObject = new GameObject($"Chunk {id.x}:{id.y}:{id.z}");
        ChunkCube chunk = gameObject.AddComponent<ChunkCube>();
        chunk.GenerateMeshesFromData(data);
        return gameObject;
    }
}
public class ChunksHolder
{
    private Dictionary<Vector3, ChunkCube> chunks = new Dictionary<Vector3, ChunkCube>();
    private List<Vector3> ids = new List<Vector3>();

    public List<Vector3> GetExistingIds()
    {
        return ids;
    }

    public List<ChunkCube> GetAllChunks()
    {
        List<ChunkCube> chunkList = new List<ChunkCube>();
        foreach (ChunkCube chunk in chunks.Values)
        {
            chunkList.Add(chunk);
        }
        return chunkList;
    }
    public void Add(Vector3 id, ChunkCube chunk)
    {
        chunks.Add(id, chunk);
        ids.Add(id);
    }

    public void Clear()
    {
        chunks.Clear();
        ids.Clear();
    }
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
    private RenderTexture m_normalsBuffer;
    public AsyncGPUReadbackRequest request;

    public ChunkGenerator()
    {

    }
    public void Init()
    {
        _initializeComputeShaders();
        _pushTablesIntoBuffers();
        _initializeSettings();
    }
    #region OneTimeInitialization
    private void _initializeComputeShaders()
    {
        m_perlinNoise = Object.Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/ImprovedPerlinNoise3D"));
        m_marchingCubes = Object.Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/MarchingCubes"));
        m_normals = Object.Instantiate(Resources.Load<ComputeShader>("Shaders/ComputeShaders/Normals"));
    }
    private void _pushTablesIntoBuffers()
    {
        m_cubeEdgeFlags = new ComputeBuffer(256, sizeof(int));
        m_cubeEdgeFlags.SetData(MarchingCubesTables.CubeEdgeFlags);
        m_triangleConnectionTable = new ComputeBuffer(256 * 16, sizeof(int));
        m_triangleConnectionTable.SetData(MarchingCubesTables.TriangleConnectionTable);
        m_marchingCubes.SetBuffer(0, "_CubeEdgeFlags", m_cubeEdgeFlags);
        m_marchingCubes.SetBuffer(0, "_TriangleConnectionTable", m_triangleConnectionTable);
    }
    #endregion
    #region OnChangedSettingsInitialization
    private void _initializeBuffers()
    {
        if (TerrainMap.instance.mapGenSettings.chunkSize % 8 != 0)
        {
            //There are 8 threads run per group so N must be divisible by 8.
            throw new System.ArgumentException("Chunk size must be divisible be 8");
        }
        #region NormalsTexture
        //Holds the normals of the voxels.
        m_normalsBuffer = new RenderTexture(TerrainMap.instance.mapGenSettings.chunkSize, TerrainMap.instance.mapGenSettings.chunkSize, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
        m_normalsBuffer.dimension = TextureDimension.Tex3D;
        m_normalsBuffer.enableRandomWrite = true;
        m_normalsBuffer.useMipMap = false;
        m_normalsBuffer.volumeDepth = TerrainMap.instance.mapGenSettings.chunkSize;
        m_normalsBuffer.Create();
        #endregion
        int chunkVolume = (int)Mathf.Pow(TerrainMap.instance.mapGenSettings.chunkSize, 3);
        int meshBufferSize = chunkVolume * 5 * 3; // there is 5 triangles each with 3 points
        m_noiseBuffer = new ComputeBuffer(chunkVolume, sizeof(float));
        m_meshBuffer = new ComputeBuffer(meshBufferSize, sizeof(float) * 7);


    }
    private void _initializePerlin()
    {
        m_perlinNoise.SetInt("_Width", TerrainMap.instance.mapGenSettings.chunkSize);
        m_perlinNoise.SetInt("_Height", TerrainMap.instance.mapGenSettings.chunkSize);
        m_perlinNoise.SetFloat("_Frequency", 0.02f);
        m_perlinNoise.SetFloat("_Lacunarity", 2.0f);
        m_perlinNoise.SetFloat("_Gain", 0.5f);
        m_perlinNoise.SetTexture(0, "_PermTable2D", PerlinAPI.perlin3D.PermutationTable2D);
        m_perlinNoise.SetTexture(0, "_Gradient3D", PerlinAPI.perlin3D.Gradient3D);
        m_perlinNoise.SetBuffer(0, "_Result", m_noiseBuffer);
    }
    private void _initializeNormals()
    {
        m_normals.SetInt("_Width", TerrainMap.instance.mapGenSettings.chunkSize);
        m_normals.SetInt("_Height", TerrainMap.instance.mapGenSettings.chunkSize);
        m_normals.SetBuffer(0, "_Noise", m_noiseBuffer);
        m_normals.SetTexture(0, "_Result", m_normalsBuffer);
    }
    private void _initializeMarchingCubes()
    {
        m_marchingCubes.SetInt("_Width", TerrainMap.instance.mapGenSettings.chunkSize);
        m_marchingCubes.SetInt("_Height", TerrainMap.instance.mapGenSettings.chunkSize);
        m_marchingCubes.SetInt("_Depth", TerrainMap.instance.mapGenSettings.chunkSize);
        m_marchingCubes.SetInt("_Border", 1);
        m_marchingCubes.SetFloat("_Target", 0.0f);
        m_marchingCubes.SetBuffer(0, "_Voxels", m_noiseBuffer);
        m_marchingCubes.SetTexture(0, "_Normals", m_normalsBuffer);
        m_marchingCubes.SetBuffer(0, "_Buffer", m_meshBuffer);

        int chunkVolume = (int)Mathf.Pow(TerrainMap.instance.mapGenSettings.chunkSize, 3);
        int meshBufferSize = chunkVolume * 5 * 3; // there is 5 triangles each with 3 points
        //Could also use the ClearMesh compute shader provided.
        float[] val = new float[meshBufferSize * 7];
        for (int i = 0; i < meshBufferSize * 7; i++)
            val[i] = -1.0f;

        m_meshBuffer.SetData(val);
    }
    private void _initializeSettings()
    {
        _initializeBuffers();
        _initializePerlin();
        _initializeNormals();
        _initializeMarchingCubes();
    }
    #endregion
    #region DispatchingComputeShaders
    private void DispatchPerlin()
    {
        m_perlinNoise.Dispatch(0, TerrainMap.instance.mapGenSettings.chunkSize / 8, TerrainMap.instance.mapGenSettings.chunkSize / 8, TerrainMap.instance.mapGenSettings.chunkSize / 8);
    }
    private void DispatchNormals()
    {
        m_normals.Dispatch(0, TerrainMap.instance.mapGenSettings.chunkSize / 8, TerrainMap.instance.mapGenSettings.chunkSize / 8, TerrainMap.instance.mapGenSettings.chunkSize / 8);
    }
    private void DispatchMarchingCubes()
    {
        m_marchingCubes.Dispatch(0, TerrainMap.instance.mapGenSettings.chunkSize / 8, TerrainMap.instance.mapGenSettings.chunkSize / 8, TerrainMap.instance.mapGenSettings.chunkSize / 8);
    }
    #endregion
    private void _setPerlinOffset()
    {
        m_perlinNoise.SetFloat("_X", offset.x);
        m_perlinNoise.SetFloat("_Y", offset.y);
        m_perlinNoise.SetFloat("_Z", offset.z);
    }
    public void SetOffset(Vector3 offset)
    {
        this.offset = offset;
    }
    public void StartGeneration()
    {
        _setPerlinOffset();
        DispatchPerlin();
        //DispatchNormals(); <- uncomment for smooth shading, we didnt implemented our own normals calc yet
        DispatchMarchingCubes();
        request = AsyncGPUReadback.Request(m_meshBuffer);
    }
    private void _returnSelfIntoPool()
    {
        TerrainMap.instance.ReturnChunkGeneratorIntoPool(this);
    }
    public IEnumerator GetGeneratedData()
    {
        while(true)
        {
            if (request.done)
            {
                if (!request.hasError)
                {
                    ChunkBuilder.CreateChunk(offset, request.GetData<Vert>().ToArray());
                    //TerrainMap.instance.AddChunk();
                    //_returnSelfIntoPool();
                    break;
                }
            }
            yield return new WaitForSeconds(.0001f);
        }
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
        _initializeSettings();
    }
}
public class MapChunkFinder
{
    private ConcurrentQueue<Vector3> idsToGenerate = new ConcurrentQueue<Vector3>();
    private List<Vector3> newChunkIds = new List<Vector3>();
    private List<Vector3> filteredChunkIds = new List<Vector3>();

    public void FindChunksAround()
    {
        _findAllChunksInRadius();
        _filterChunksOut();
        idsToGenerate.Enqueue(filteredChunkIds);
    }
    private void _findAllChunksInRadius()
    {
        newChunkIds = FindChunkIdsAroundAPI.FindChunksIdsAroundSquare(PlayerAPI.GetPlayerPosition(), TerrainMap.instance.mapGenSettings.squareGenRadius, TerrainMap.instance.mapGenSettings.chunkSize);
    }
    private void _filterChunksOut()
    {
        filteredChunkIds.Clear();
        foreach(Vector3 chunkId in newChunkIds)
        {
            if (!TerrainMap.instance.GetExistingIds().Contains(chunkId))
            {
                filteredChunkIds.Add(chunkId);
            }
        }
    }
    public bool GetOneIdToGenerate(out Vector3 _id)
    {
        return idsToGenerate.TryDequeue(out _id);
    }
}
public class TerrainMap : MonoBehaviour
{
    public static TerrainMap instance;
    public GameObject chunkMeshPrefab;
    public MapGenSettings mapGenSettings;
    private ChunksHolder chunks = new ChunksHolder();
    private ObjectPool<ChunkGenerator> chunkGenerators;
    private MapChunkFinder mapChunkFinder = new MapChunkFinder();
    public int chunkGeneratorPoolSize = 5;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //mapGenSettings = Resources.Load<MapGenSettings>("Assets/Resources/ScriptableObjects/MapSettings/MapSettings.asset");
    }
    void Start()
    {
        PerlinAPI.instance.seed = mapGenSettings.seed;
        PerlinAPI.ReloadPerlin3D();
        PreparePools();
        StartThreads();
    }
    void Update()
    {
        GenerationPipeline();
    }
    private void StartThreads()
    {
        //Thread threadGen = new Thread(() => GenThread());
        //threadGen.Start();
    }
    private void GenerationPipeline()
    {
        //1.find chunks to generate
        mapChunkFinder.FindChunksAround();
        //2.seed generators
        ChunkGenerator chunkGenerator;
        while (chunkGenerators.GetOne(out chunkGenerator))
        {
            Vector3 chunkId;
            if (mapChunkFinder.GetOneIdToGenerate(out chunkId))
            {
                chunkGenerator.SetOffset(chunkId);
                chunkGenerator.StartGeneration();
                StartCoroutine(chunkGenerator.GetGeneratedData());
            }
            else
            {
                chunkGenerators.ReturnIntoPool(chunkGenerator);
                break;
            }
        }
    }
    private void OnDestroy()
    {
        ClearPools();
        DestroyEveryChunk();
    }
    private void PreparePools()
    {
        chunkGenerators = new ObjectPool<ChunkGenerator>(chunkGeneratorPoolSize);
    }
    public void DestroyEveryChunk()
    {
        foreach (ChunkCube chunk in chunks.GetAllChunks())
        {
            Destroy(chunk);
        }
        chunks.Clear();
    }
    private void ClearPools()
    {
        chunkGenerators.ClearPool();
    }
    public List<Vector3> GetExistingIds()
    {
        return chunks.GetExistingIds();
    }
    public void ReturnChunkGeneratorIntoPool(ChunkGenerator chunkGenerator)
    {
        chunkGenerators.ReturnIntoPool(chunkGenerator);
    }
    public void AddChunk(GameObject chunk)
    {
        ChunkCube chunkCube = chunk.GetComponent<ChunkCube>();
        if (!chunks.GetExistingIds().Contains(chunkCube.id))
        {
            chunk.transform.parent = transform;
            chunks.Add(chunkCube.id, chunkCube);
        }
        else
        {
            Destroy(chunk);
        }
    }
}
